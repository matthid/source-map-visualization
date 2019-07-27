// ts2fable 0.6.2
module rec Fable.MyImport.SourceMap
open System
open Fable.Core
open Fable.Core.JS

let [<Import("SourceMapConsumer","source-map")>] SourceMapConsumer: SourceMapConsumerConstructor = jsNative
let [<Import("BasicSourceMapConsumer","source-map")>] BasicSourceMapConsumer: BasicSourceMapConsumerConstructor = jsNative
let [<Import("IndexedSourceMapConsumer","source-map")>] IndexedSourceMapConsumer: IndexedSourceMapConsumerConstructor = jsNative
let [<Import("default","source-map")>] sourceMap: IExports = jsNative

type [<AllowNullLiteral>] IExports =
    abstract SourceMapConsumer: SourceMapConsumerConstructorStatic
    abstract BasicSourceMapConsumer: BasicSourceMapConsumerConstructorStatic
    abstract IndexedSourceMapConsumer: IndexedSourceMapConsumerConstructorStatic
    abstract SourceMapGenerator: SourceMapGeneratorStatic
    abstract SourceNode: SourceNodeStatic

type SourceMapUrl =
    string

type [<AllowNullLiteral>] StartOfSourceMap =
    abstract file: string option with get, set
    abstract sourceRoot: string option with get, set
    abstract skipValidation: bool option with get, set

type [<AllowNullLiteral>] RawSourceMap =
    abstract version: float with get, set
    abstract sources: ResizeArray<string> with get, set
    abstract names: ResizeArray<string> with get, set
    abstract sourceRoot: string option with get, set
    abstract sourcesContent: ResizeArray<string> option with get, set
    abstract mappings: string with get, set
    abstract file: string with get, set

type [<AllowNullLiteral>] RawIndexMap =
    inherit StartOfSourceMap
    abstract version: float with get, set
    abstract sections: ResizeArray<RawSection> with get, set

type [<AllowNullLiteral>] RawSection =
    abstract offset: Position with get, set
    abstract map: RawSourceMap with get, set

type [<AllowNullLiteral>] Position =
    abstract line: float with get, set
    abstract column: float with get, set

type [<AllowNullLiteral>] NullablePosition =
    abstract line: float option with get, set
    abstract column: float option with get, set
    abstract lastColumn: float option with get, set

type [<AllowNullLiteral>] MappedPosition =
    abstract source: string with get, set
    abstract line: float with get, set
    abstract column: float with get, set
    abstract name: string option with get, set

type [<AllowNullLiteral>] NullableMappedPosition =
    abstract source: string option with get, set
    abstract line: float option with get, set
    abstract column: float option with get, set
    abstract name: string option with get, set

type [<AllowNullLiteral>] MappingItem =
    abstract source: string with get, set
    abstract generatedLine: float with get, set
    abstract generatedColumn: float with get, set
    abstract originalLine: float with get, set
    abstract originalColumn: float with get, set
    abstract name: string with get, set

type [<AllowNullLiteral>] Mapping =
    abstract generated: Position with get, set
    abstract original: Position with get, set
    abstract source: string with get, set
    abstract name: string option with get, set

type [<AllowNullLiteral>] CodeWithSourceMap =
    abstract code: string with get, set
    abstract map: SourceMapGenerator with get, set

type [<AllowNullLiteral>] SourceMapConsumer =
    /// Compute the last column for each generated mapping. The last column is
    /// inclusive.
    abstract computeColumnSpans: unit -> unit
    /// Returns the original source, line, and column information for the generated
    /// source's line and column positions provided. The only argument is an object
    /// with the following properties:
    /// 
    ///    - line: The line number in the generated source.
    ///    - column: The column number in the generated source.
    ///    - bias: Either 'SourceMapConsumer.GREATEST_LOWER_BOUND' or
    ///      'SourceMapConsumer.LEAST_UPPER_BOUND'. Specifies whether to return the
    ///      closest element that is smaller than or greater than the one we are
    ///      searching for, respectively, if the exact element cannot be found.
    ///      Defaults to 'SourceMapConsumer.GREATEST_LOWER_BOUND'.
    /// 
    /// and an object is returned with the following properties:
    /// 
    ///    - source: The original source file, or null.
    ///    - line: The line number in the original source, or null.
    ///    - column: The column number in the original source, or null.
    ///    - name: The original identifier, or null.
    abstract originalPositionFor: generatedPosition: obj -> NullableMappedPosition
    /// Returns the generated line and column information for the original source,
    /// line, and column positions provided. The only argument is an object with
    /// the following properties:
    /// 
    ///    - source: The filename of the original source.
    ///    - line: The line number in the original source.
    ///    - column: The column number in the original source.
    ///    - bias: Either 'SourceMapConsumer.GREATEST_LOWER_BOUND' or
    ///      'SourceMapConsumer.LEAST_UPPER_BOUND'. Specifies whether to return the
    ///      closest element that is smaller than or greater than the one we are
    ///      searching for, respectively, if the exact element cannot be found.
    ///      Defaults to 'SourceMapConsumer.GREATEST_LOWER_BOUND'.
    /// 
    /// and an object is returned with the following properties:
    /// 
    ///    - line: The line number in the generated source, or null.
    ///    - column: The column number in the generated source, or null.
    abstract generatedPositionFor: originalPosition: obj -> NullablePosition
    /// Returns all generated line and column information for the original source,
    /// line, and column provided. If no column is provided, returns all mappings
    /// corresponding to a either the line we are searching for or the next
    /// closest line that has any mappings. Otherwise, returns all mappings
    /// corresponding to the given line and either the column we are searching for
    /// or the next closest column that has any offsets.
    /// 
    /// The only argument is an object with the following properties:
    /// 
    ///    - source: The filename of the original source.
    ///    - line: The line number in the original source.
    ///    - column: Optional. the column number in the original source.
    /// 
    /// and an array of objects is returned, each with the following properties:
    /// 
    ///    - line: The line number in the generated source, or null.
    ///    - column: The column number in the generated source, or null.
    abstract allGeneratedPositionsFor: originalPosition: MappedPosition -> ResizeArray<NullablePosition>
    /// Return true if we have the source content for every source in the source
    /// map, false otherwise.
    abstract hasContentsOfAllSources: unit -> bool
    /// Returns the original source content. The only argument is the url of the
    /// original source file. Returns null if no original source content is
    /// available.
    abstract sourceContentFor: source: string * ?returnNullOnMissing: bool -> string option
    /// <summary>Iterate over each mapping between an original source/line/column and a
    /// generated line/column in this source map.</summary>
    /// <param name="callback">The function that is called with each mapping.</param>
    /// <param name="context">Optional. If specified, this object will be the value of `this` every
    /// time that `aCallback` is called.</param>
    /// <param name="order">Either `SourceMapConsumer.GENERATED_ORDER` or
    /// `SourceMapConsumer.ORIGINAL_ORDER`. Specifies whether you want to
    /// iterate over the mappings sorted by the generated file's line/column
    /// order or the original's source/line/column order, respectively. Defaults to
    /// `SourceMapConsumer.GENERATED_ORDER`.</param>
    abstract eachMapping: callback: (MappingItem -> unit) * ?context: obj * ?order: float -> unit
    /// Free this source map consumer's associated wasm data that is manually-managed.
    /// Alternatively, you can use SourceMapConsumer.with to avoid needing to remember to call destroy.
    abstract destroy: unit -> unit

type [<AllowNullLiteral>] SourceMapConsumerConstructor =
    abstract prototype: SourceMapConsumer with get, set
    abstract GENERATED_ORDER: float with get, set
    abstract ORIGINAL_ORDER: float with get, set
    abstract GREATEST_LOWER_BOUND: float with get, set
    abstract LEAST_UPPER_BOUND: float with get, set
    /// <summary>Create a BasicSourceMapConsumer from a SourceMapGenerator.</summary>
    /// <param name="sourceMap">The source map that will be consumed.</param>
    abstract fromSourceMap: sourceMap: SourceMapGenerator * ?sourceMapUrl: SourceMapUrl -> Promise<BasicSourceMapConsumer>
    /// Construct a new `SourceMapConsumer` from `rawSourceMap` and `sourceMapUrl`
    /// (see the `SourceMapConsumer` constructor for details. Then, invoke the `async
    /// function f(SourceMapConsumer) -> T` with the newly constructed consumer, wait
    /// for `f` to complete, call `destroy` on the consumer, and return `f`'s return
    /// value.
    /// 
    /// You must not use the consumer after `f` completes!
    /// 
    /// By using `with`, you do not have to remember to manually call `destroy` on
    /// the consumer, since it will be called automatically once `f` completes.
    /// 
    /// ```js
    /// const xSquared = await SourceMapConsumer.with(
    ///    myRawSourceMap,
    ///    null,
    ///    async function (consumer) {
    ///      // Use `consumer` inside here and don't worry about remembering
    ///      // to call `destroy`.
    /// 
    ///      const x = await whatever(consumer);
    ///      return x * x;
    ///    }
    /// );
    /// 
    /// // You may not use that `consumer` anymore out here; it has
    /// // been destroyed. But you can use `xSquared`.
    /// console.log(xSquared);
    /// ```
    abstract ``with``: rawSourceMap: U3<RawSourceMap, RawIndexMap, string> * sourceMapUrl: SourceMapUrl option * callback: (U2<BasicSourceMapConsumer, IndexedSourceMapConsumer> -> U2<Promise<'T>, 'T>) -> Promise<'T>

type [<AllowNullLiteral>] SourceMapConsumerConstructorStatic =
    [<Emit "new $0($1...)">] abstract Create: rawSourceMap: RawSourceMap * ?sourceMapUrl: SourceMapUrl -> SourceMapConsumerConstructor
    [<Emit "new $0($1...)">] abstract Create: rawSourceMap: RawIndexMap * ?sourceMapUrl: SourceMapUrl -> SourceMapConsumerConstructor
    [<Emit "new $0($1...)">] abstract Create: rawSourceMap: U3<RawSourceMap, RawIndexMap, string> * ?sourceMapUrl: SourceMapUrl -> SourceMapConsumerConstructor

type [<AllowNullLiteral>] BasicSourceMapConsumer =
    inherit SourceMapConsumer
    abstract file: string with get, set
    abstract sourceRoot: string with get, set
    abstract sources: ResizeArray<string> with get, set
    abstract sourcesContent: ResizeArray<string> with get, set

type [<AllowNullLiteral>] BasicSourceMapConsumerConstructor =
    abstract prototype: BasicSourceMapConsumer with get, set
    /// <summary>Create a BasicSourceMapConsumer from a SourceMapGenerator.</summary>
    /// <param name="sourceMap">The source map that will be consumed.</param>
    abstract fromSourceMap: sourceMap: SourceMapGenerator -> Promise<BasicSourceMapConsumer>

type [<AllowNullLiteral>] BasicSourceMapConsumerConstructorStatic =
    [<Emit "new $0($1...)">] abstract Create: rawSourceMap: U2<RawSourceMap, string> -> BasicSourceMapConsumerConstructor

type [<AllowNullLiteral>] IndexedSourceMapConsumer =
    inherit SourceMapConsumer
    abstract sources: ResizeArray<string> with get, set

type [<AllowNullLiteral>] IndexedSourceMapConsumerConstructor =
    abstract prototype: IndexedSourceMapConsumer with get, set

type [<AllowNullLiteral>] IndexedSourceMapConsumerConstructorStatic =
    [<Emit "new $0($1...)">] abstract Create: rawSourceMap: U2<RawIndexMap, string> -> IndexedSourceMapConsumerConstructor

type [<AllowNullLiteral>] SourceMapGenerator =
    /// Add a single mapping from original source line and column to the generated
    /// source's line and column for this source map being created. The mapping
    /// object should have the following properties:
    /// 
    ///    - generated: An object with the generated line and column positions.
    ///    - original: An object with the original line and column positions.
    ///    - source: The original source file (relative to the sourceRoot).
    ///    - name: An optional original token name for this mapping.
    abstract addMapping: mapping: Mapping -> unit
    /// Set the source content for a source file.
    abstract setSourceContent: sourceFile: string * sourceContent: string -> unit
    /// <summary>Applies the mappings of a sub-source-map for a specific source file to the
    /// source map being generated. Each mapping to the supplied source file is
    /// rewritten using the supplied source map. Note: The resolution for the
    /// resulting mappings is the minimium of this map and the supplied map.</summary>
    /// <param name="sourceMapConsumer">The source map to be applied.</param>
    /// <param name="sourceFile">Optional. The filename of the source file.
    /// If omitted, SourceMapConsumer's file property will be used.</param>
    /// <param name="sourceMapPath">Optional. The dirname of the path to the source map
    /// to be applied. If relative, it is relative to the SourceMapConsumer.
    /// This parameter is needed when the two source maps aren't in the same
    /// directory, and the source map to be applied contains relative source
    /// paths. If so, those relative source paths need to be rewritten
    /// relative to the SourceMapGenerator.</param>
    abstract applySourceMap: sourceMapConsumer: SourceMapConsumer * ?sourceFile: string * ?sourceMapPath: string -> unit
    abstract toString: unit -> string
    abstract toJSON: unit -> RawSourceMap

type [<AllowNullLiteral>] SourceMapGeneratorStatic =
    [<Emit "new $0($1...)">] abstract Create: ?startOfSourceMap: StartOfSourceMap -> SourceMapGenerator
    /// <summary>Creates a new SourceMapGenerator based on a SourceMapConsumer</summary>
    /// <param name="sourceMapConsumer">The SourceMap.</param>
    abstract fromSourceMap: sourceMapConsumer: SourceMapConsumer -> SourceMapGenerator

type [<AllowNullLiteral>] SourceNode =
    abstract children: ResizeArray<SourceNode> with get, set
    abstract sourceContents: obj option with get, set
    abstract line: float with get, set
    abstract column: float with get, set
    abstract source: string with get, set
    abstract name: string with get, set
    abstract add: chunk: U3<ResizeArray<U2<string, SourceNode>>, SourceNode, string> -> SourceNode
    abstract prepend: chunk: U3<ResizeArray<U2<string, SourceNode>>, SourceNode, string> -> SourceNode
    abstract setSourceContent: sourceFile: string * sourceContent: string -> unit
    abstract walk: fn: (string -> MappedPosition -> unit) -> unit
    abstract walkSourceContents: fn: (string -> string -> unit) -> unit
    abstract join: sep: string -> SourceNode
    abstract replaceRight: pattern: string * replacement: string -> SourceNode
    abstract toString: unit -> string
    abstract toStringWithSourceMap: ?startOfSourceMap: StartOfSourceMap -> CodeWithSourceMap

type [<AllowNullLiteral>] SourceNodeStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> SourceNode
    [<Emit "new $0($1...)">] abstract Create: line: float option * column: float option * source: string option * ?chunks: U3<ResizeArray<U2<string, SourceNode>>, SourceNode, string> * ?name: string -> SourceNode
    abstract fromStringWithSourceMap: code: string * sourceMapConsumer: SourceMapConsumer * ?relativePath: string -> SourceNode