module SourceMapViewer

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fulma
open System
open Shared
open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop
open Fable.MyImport

type Generated =
    abstract files : string with get,set
    abstract mappings : string with get,set

let [<Import("default","./../../app/generateHtml")>] generateHtml: Func<SourceMap.SourceMapConsumerConstructor, string, string ResizeArray, Generated> = jsNative

let [<Import("default","./setupCallbacks")>] setupCallbacks: Func<unit> = jsNative

let css : obj = importAll "./../../app/app.less"

type Model = { 
    File: SourceMapFile;
    Html: Generated }

type Msg =
    | MouseHover


(*
        var exampleJs = require("!raw!../../../build/fable-library/Array.js");
        var exampleMap = require("!json!../../../build/fable-library/Array.js.map");
        var sources = [require("!raw!../../../src/fable-library/Array.fs")];
        exampleMap.file = "Array.js";*)
let init (f:SourceMapFile) : Model * Cmd<Msg> =
    let js = f.converted
    let source = f.original
    let map : SourceMap.RawSourceMap = JSON.parse(f.sourceMap) :?> _
    map.file <- f.name
    let sm = SourceMap.sourceMap.SourceMapConsumer.Create(map)
    let html = generateHtml.Invoke(sm, js, ResizeArray [source])
    setupCallbacks.Invoke()
    { File = f; Html = html }, Cmd.none

let update msg model =
    model, Cmd.none

let view model dispatch =
    //let filesElem = div [ DangerouslySetInnerHTML { __html = model.Html.files } ] []
    //let mappingsElem = div [ DangerouslySetInnerHTML { __html = model.Html.mappings } ] []
    div [ ClassName "full-screen" ] 
        [ //header [] []
          main [DangerouslySetInnerHTML { __html = model.Html.files } ] [ ]
          footer [
            Style [ Overflow "scroll" ]
            DangerouslySetInnerHTML { __html = model.Html.mappings }] [] ]