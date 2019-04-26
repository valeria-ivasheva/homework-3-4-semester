module SeqInfinite

/// Последовательность [1, -1, 1, -1, 1, -1, 1, -1, …]
let seqSimple = Seq.initInfinite (fun index -> if (index % 2 = 0) then 1 else -1 )

/// Последовательность [1, -2, 3, -4, 5, -6, …]
let seqNeed = Seq.initInfinite (fun index -> (index + 1) * (Seq.tryItem index seqSimple).Value)