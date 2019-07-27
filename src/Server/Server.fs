module Server

open System
open System.IO
open System.Threading.Tasks

open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop
open Node
open Fable.MyImport

open Shared

#if FABLE_COMPILER
let tryGetEnv = (fun (e:string) -> ``process``.env?(e)) >> function null | "" -> None | x -> Some x
#else
let tryGetEnv = System.Environment.GetEnvironmentVariable >> function null | "" -> None | x -> Some x
#endif

let port =
    "SERVER_PORT"
    |> tryGetEnv |> Option.map uint16 |> Option.defaultValue 8085us

let createSourceMapFile (name:string) (original:string) (converted:string) (sourceMap:string) =
    //let x = createEmpty<SourceMapFile>
    //x?("name") <- name
    //x?("converted") <- converted
    //x?("sourceMap") <- sourceMap
    //x?("original") <- original
    //x
    { name = name; converted = converted; sourceMap = sourceMap; original = original }

let startServer clientDir (port:uint16) (files:SourceMapFile ResizeArray) =
    let app = express.Invoke()

    //app.get(!^ "/", (fun req (res:express.Response) cb -> res.send("Hello World!") :> obj)) |> ignore<express.Application>
    app.``use``(express.``static``(clientDir)) |> ignore<express.Application>
    let fileContents =
        ResizeArray(
            files
            |> Seq.map (fun f ->
                createSourceMapFile f.name (fs.readFileSync(f.original, "utf8")) (fs.readFileSync(f.converted, "utf8")) (fs.readFileSync(f.sourceMap, "utf8")) ))
    app.get(!^ "/api/sourceMapFiles", (fun req (res:express.Response) cb ->
        res.json(fileContents) :> obj
    ))  |> ignore<express.Application>
    
    app.listen(port, fun () -> Fable.Core.JS.console.log(sprintf "Source map visualization listening on port %d, -> %s!" port clientDir)) |> ignore<obj>

let startServerWithArgs clientDir (argv:string[]) =
    let files =
        argv
        |> Seq.filter (fun arg -> arg.StartsWith "--sm=")
        |> Seq.map (fun arg ->
            let trimmed = arg.Substring ("--sm=".Length)
            let splitted = trimmed.Split(';')
            if splitted.Length < 3 then
                failwithf "argument '%s' is incomplete" arg
            createSourceMapFile
                (if splitted.Length > 3 then splitted.[3] else splitted.[0])
                splitted.[0]
                splitted.[1]
                splitted.[2])
        |> ResizeArray
    let port =
        match argv
              |> Seq.tryFind (fun arg -> arg.StartsWith "--port=") with
        | Some arg ->
            UInt16.Parse(arg.Substring("--port=".Length))
        | None -> port
    startServer clientDir port files
    0


type IExports =
    abstract StartServer : clientDir:string -> port:uint16 -> files:SourceMapFile ResizeArray -> unit
    abstract startServerWithArgs : clientDir:string -> argv:string[] -> int

Fable.Core.JsInterop.exportDefault
    { new IExports with
        member this.StartServer clientDir port files =
            startServer clientDir port files
        member this.startServerWithArgs clientDir argv =
            startServerWithArgs clientDir argv
    }

(*
startServer publicPath port (ResizeArray [
    createSourceMapFile
        "fable-library/Array.fs"
        @"C:\proj\Fable\src\fable-library\Array.fs"
        @"C:\proj\Fable\build\fable-library\Array.js"
        @"C:\proj\Fable\build\fable-library\Array.js.map"])
*)

    