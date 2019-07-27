module Client

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json
open Fable.MyImport

open Shared

// The model holds data that you want to keep track of while the application is running
// in this case, we are keeping track of a counter
// we mark it as optional, because initially it will not be available from the client
// the initial value will be requested from server
type Model = { 
    Files: SourceMapFile ResizeArray option 
    SelectedFileComponentModel : SourceMapViewer.Model option }

// The Msg type defines what events/actions can occur while the application is running
// the state of the application changes *only* in reaction to these events
type Msg =
    | DataLoaded of SourceMapFile ResizeArray
    | SelectedFileComponentMsg of SourceMapViewer.Msg
    | SelectFile of string

let initialFiles () = Fetch.fetchAs<SourceMapFile ResizeArray> "/api/sourceMapFiles"

// defines the initial state and initial command (= side-effect) of the application
let init () : Model * Cmd<Msg> =
    let initialModel = { Files = None; SelectedFileComponentModel = None }
    let loadCountCmd =
        Cmd.OfPromise.perform initialFiles () DataLoaded
    initialModel, loadCountCmd

// The update function computes the next state of the application based on the current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg, currentModel with
    | DataLoaded initialFiles, _ ->
        let nextModel = { currentModel with Files = Some initialFiles }
        nextModel, Cmd.none
    | SelectFile fname, { Files = Some files } ->
        match files |> Seq.tryFind (fun f -> f.name = fname) with
        | Some findFile ->
            let newModel, modelCmd = SourceMapViewer.init findFile
            { currentModel with SelectedFileComponentModel = Some newModel }, Cmd.map SelectedFileComponentMsg modelCmd
        | None -> currentModel, Cmd.none
    | SelectedFileComponentMsg componentMsg, { SelectedFileComponentModel = Some componentModel } ->
        let newModel, modelCmd = SourceMapViewer.update componentMsg componentModel
        { currentModel with SelectedFileComponentModel = Some newModel }, Cmd.map SelectedFileComponentMsg modelCmd
    | _ -> currentModel, Cmd.none


let safeComponents =
    let components =
        span [ ]
           [ a [ Href "https://github.com/SAFE-Stack/SAFE-template" ]
               [ str "SAFE  "
                 str Version.template ]
             str ", "
             a [ Href "https://github.com/giraffe-fsharp/Giraffe" ] [ str "Giraffe" ]
             str ", "
             a [ Href "http://fable.io" ] [ str "Fable" ]
             str ", "
             a [ Href "https://elmish.github.io" ] [ str "Elmish" ]
             str ", "
             a [ Href "https://fulma.github.io/Fulma" ] [ str "Fulma" ]

           ]

    span [ ]
        [ str "Version "
          strong [ ] [ str Version.app ]
          str " powered by: "
          components ]

let viewFiles (files:ResizeArray<SourceMapFile>) model dispatch =
    div [ Style [ Display DisplayOptions.Flex; FlexDirection "column"; MinHeight "10px" ] ]
        [ yield Content.content [ Content.Modifiers [Modifier.TextAlignment (Screen.All, TextAlignment.Centered)]] [ str (sprintf "%d files loaded" files.Count)]
          
          yield
            Content.content [ Content.Modifiers [Modifier.TextAlignment (Screen.All, TextAlignment.Centered)]]
               (files
                |> Seq.map (fun f -> button [ OnClick (fun _ -> dispatch (SelectFile f.name)) ] [ str f.name ] )
                |> Seq.toList)
          yield
            match model with
            | { SelectedFileComponentModel = None } ->
                Content.content [ Content.Modifiers [Modifier.TextAlignment (Screen.All, TextAlignment.Centered)]] 
                    [ str "Please select a file" ]
            | { SelectedFileComponentModel = Some componentModel } ->
                SourceMapViewer.view componentModel (dispatch << SelectedFileComponentMsg) ]

let show model dispatch =
    match model with
    | { Files = Some files } -> viewFiles files model dispatch
    | { Files = None  } ->
        Content.content [ Content.Modifiers [Modifier.TextAlignment (Screen.All, TextAlignment.Centered)]]
            [ str "Loading..." ]

let button txt onClick =
    Button.button
        [ Button.IsFullWidth
          Button.Color IsPrimary
          Button.OnClick onClick ]
        [ str txt ]

let view (model : Model) (dispatch : Msg -> unit) =
    div [ Style [ Height "100%"; Display DisplayOptions.Flex; FlexDirection "column" ]]
        [ Navbar.navbar [ Navbar.Color IsPrimary ]
            [ Navbar.Item.div [ ]
                [ Heading.h2 [ ]
                    [ str "Source map Visualization" ] ] ]

          show model dispatch

          Footer.footer [ Props [ Style [ Height "50px"; Padding "1rem" ] ] ]
                [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [ safeComponents ] ] ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
