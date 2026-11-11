// Purpose: Score the real AddNumbers tool against a small fixed evaluation dataset.
// Runs offline with no model or credentials; this is not a Foundry cloud evaluation.
// Expected result: per-case actual/expected values and exact-match accuracy.

// Step 1: Keep the labeled evaluation inputs visible for the audience.
(int A, int B, int Expected)[] dataset = [(19, 23, 42), (-5, 8, 3), (0, 0, 0)];
int matched = 0;

// Step 2: Invoke the real function rather than scoring hard-coded output.
foreach (var (a, b, expected) in dataset)
{
    int actual = DemoTools.AddNumbers(a, b);
    bool exactMatch = actual == expected;
    if (exactMatch)
        matched++;
    Console.WriteLine($"Input: {a}, {b} | Expected: {expected} | Actual: {actual} | Match: {exactMatch}");
}

// Step 3: This metric covers tool arithmetic, not model quality or tool selection.
Console.WriteLine($"Exact-match accuracy: {(double)matched / dataset.Length:P0} ({matched}/{dataset.Length})");
