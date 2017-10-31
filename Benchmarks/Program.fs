open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open System
open ExtCore.Control

[<MemoryDiagnoser>]
type Test() =
    let choiceFun x y = x |> Choice.bind (fun _ -> y)
    let resultFun x y = x |> Result.bind (fun _ -> y)

    [<Benchmark(Baseline = true)>]
    member __.Choice() : unit =
        choice {
            let! x = choiceFun (Choice1Of2 0) (Choice1Of2 0)
            let! y = choiceFun (Choice1Of2 1) (Choice1Of2 1)
            let! z = choiceFun (Choice1Of2 2) (Choice1Of2 2)
            let! h = choiceFun (Choice1Of2 3) (Choice1Of2 3)
            return! Choice1Of2 (x + y + z + h)
        } |> ignore


    [<Benchmark>]
    member __.Result() : unit =
        result {
            let! x = resultFun (Ok 0) (Ok 0)
            let! y = resultFun (Ok 1) (Ok 1)
            let! z = resultFun (Ok 2) (Ok 2)
            let! h = resultFun (Ok 3) (Ok 3)
            return! Ok (x + y + z + h)
        } |> ignore

[<EntryPoint>]
let main _ =
    printfn "%O" (BenchmarkRunner.Run<Test>())
    Console.ReadKey() |> ignore
    0
