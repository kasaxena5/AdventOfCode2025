import pulp
import os
import numpy as np
from typing import List, Optional, Tuple

def compute_column_upper_bounds(A: np.ndarray, b: np.ndarray) -> List[int]:
    """
    Compute a tight nonnegative upper bound for each variable x_j:
      ub_j = min_{i: A[i,j] = 1} b[i], or 0 if column j is all zeros.
    Fallback to global max(b) if needed.
    """
    m, n = A.shape
    global_ub = int(b.max()) if b.size > 0 else 0
    ubs = []
    for j in range(n):
        rows_with_one = np.where(A[:, j] == 1)[0]
        if rows_with_one.size == 0:
            ubs.append(0)  # column is all zeros → x_j must be 0
        else:
            min_b = int(b[rows_with_one].min())
            ubs.append(max(0, min_b))
    # Ensure bounds are at least 0 and not absurdly large
    ubs = [max(0, ub) for ub in ubs]
    # Optional: cap by global_ub to avoid overly large bounds
    ubs = [min(ub, global_ub) for ub in ubs]
    return ubs

def solve_min_sum_ilp(
    A: np.ndarray,
    b: np.ndarray,
    upper_bounds: Optional[List[int]] = None,
    solver: Optional[pulp.LpSolverDefault] = None,
    msg: bool = False,
) -> Tuple[Optional[np.ndarray], Optional[float], str]:
    """
    Solve: minimize sum(x_j) s.t. A x = b, x_j ∈ Z_{>=0}.
    
    Parameters
    ----------
    A : (m x n) numpy array of ints (0/1 expected)
    b : length-m numpy array of ints
    upper_bounds : optional list of length n with upper bounds on x_j
                   If None, bounds are computed from A and b.
    solver : optional PuLP solver instance (e.g., pulp.PULP_CBC_CMD())
    msg : whether to display solver logs

    Returns
    -------
    x_opt : optimal integer solution vector as numpy array (length n), or None if infeasible
    obj   : optimal objective value (sum of x_j), or None if infeasible
    status: string status ("Optimal", "Feasible", "Infeasible", etc.)
    """
    # Basic validation
    if A.ndim != 2:
        raise ValueError("A must be a 2D array.")
    m, n = A.shape
    if b.shape != (m,):
        raise ValueError(f"b must have shape ({m},).")
    if not np.issubdtype(A.dtype, np.integer) or not np.issubdtype(b.dtype, np.integer):
        raise ValueError("A and b must be integer arrays.")
    if (b < 0).any():
        # With x >= 0 and A >= 0, negative b makes the model infeasible
        return None, None, "Infeasible: b has negative entries with nonnegative x."

    A = A.astype(int)
    b = b.astype(int)

    # Upper bounds
    if upper_bounds is None:
        upper_bounds = compute_column_upper_bounds(A, b)
    if len(upper_bounds) != n:
        raise ValueError("upper_bounds must have length equal to number of columns in A.")
    upper_bounds = [int(max(0, ub)) for ub in upper_bounds]

    # Build model
    model = pulp.LpProblem("Min_Sum_Nonneg_Integer", pulp.LpMinimize)

    # Decision variables: x_j ∈ {0,1,2,...,UB_j}
    x_vars = [
        pulp.LpVariable(f"x_{j}", lowBound=0, upBound=upper_bounds[j], cat="Integer")
        for j in range(n)
    ]

    # Objective: minimize sum(x_j)
    model += pulp.lpSum(x_vars)

    # Constraints: Ax = b
    for i in range(m):
        model += (pulp.lpSum(A[i, j] * x_vars[j] for j in range(n)) == b[i]), f"row_{i}"

    # Select solver
    if solver is None:
        solver = pulp.PULP_CBC_CMD(msg=msg)  # CBC MILP solver

    # Solve
    result_status = model.solve(solver)
    status_str = pulp.LpStatus[result_status]

    if status_str in ("Optimal", "Feasible"):
        x_opt = np.array([int(pulp.value(xj)) for xj in x_vars], dtype=int)
        obj = float(pulp.value(model.objective))
        return x_opt, obj, status_str
    else:
        return None, None, status_str


if __name__ == "__main__":
    """
    Input format example:
    [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
    [...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
    [.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}

    Input is taken from data.txt file in the same directory.
    """
    # Read file
    file_path = os.path.join(os.path.dirname(__file__), "data.txt")
    lines = []
    with open(file_path, "r") as f:
        lines = f.readlines()
    
    # Parse input
    buttons = []
    joltages = []
    for line in lines:
        line = line.strip()
        if not line:
            continue
        parts = line.split(" ")
        button_part = parts[0]
        button_indices = []
        for part in parts[1:-1]:
            indices = part.strip("()").split(",")
            button_indices.append([int(idx) for idx in indices if idx.isdigit()])
        joltage_part = parts[-1]
        joltage_values = joltage_part.strip("{}").split(",")
        joltages.append([int(val) for val in joltage_values if val.isdigit()])
        buttons.append(button_indices)
    
    print("Parsed Buttons:", buttons)
    print("Parsed Joltages:", joltages)

    q = len(joltages)
    sum = 0
    for idx in range(q):
        # Construct A and b for testing
        # create an np matrix of size buttons length columns and joltages length rows
        m = len(joltages[idx])
        n = len(buttons[idx])
        A = np.zeros((m, n), dtype=int)
        
        # Fill A matrix, 1 if button j affects joltage i i.e j in buttons[idx][j] A[i][j] = 1
        for j in range(n):
            for i in buttons[idx][j]:
                A[i, j] = 1

        # b array is just joltages 
        b = np.array(joltages[idx], dtype=int)

        x_opt, obj, status = solve_min_sum_ilp(A, b, msg=False)
        print("Status:", status)
        if x_opt is not None:
            print("Optimal objective (sum x):", obj)
            print("x* =", x_opt)
            sum += obj
        else:
            print("No feasible solution found.")
    print("Total sum of optimal objectives:", sum)