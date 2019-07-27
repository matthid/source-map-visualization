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
let publicPath = path.resolve "../Client/deploy"
#else
let tryGetEnv = System.Environment.GetEnvironmentVariable >> function null | "" -> None | x -> Some x
let publicPath = Path.GetFullPath "../Client/deploy"
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

let startServer (port:uint16) (files:SourceMapFile ResizeArray) =
    let app = express.Invoke()

    //app.get(!^ "/", (fun req (res:express.Response) cb -> res.send("Hello World!") :> obj)) |> ignore<express.Application>
    app.``use``(express.``static``(publicPath)) |> ignore<express.Application>
    let fileContents =
        ResizeArray(
            files
            |> Seq.map (fun f ->
                createSourceMapFile f.name (fs.readFileSync(f.original, "utf8")) (fs.readFileSync(f.converted, "utf8")) (fs.readFileSync(f.sourceMap, "utf8")) ))
    app.get(!^ "/api/sourceMapFiles", (fun req (res:express.Response) cb ->
        res.json(fileContents) :> obj
    ))  |> ignore<express.Application>
    
    app.listen(port, fun () -> Fable.Core.JS.console.log(sprintf "Source map visualization listening on port %d, -> %s!" port publicPath)) |> ignore<obj>



startServer port (ResizeArray [
    createSourceMapFile
        "fable-library/Array.fs"
        @"C:\proj\Fable\src\fable-library\Array.fs"
        @"C:\proj\Fable\build\fable-library\Array.js"
        @"C:\proj\Fable\build\fable-library\Array.js.map"])