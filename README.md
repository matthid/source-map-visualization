# Source Map Visualizer

Initial idea from https://github.com/sokra/source-map-visualization

This app allows to debug and visualize source maps.


## Usage (cli tool)

### Global installation

```
npm install @matthid/source-map-visualization -g 
```

Starts a webserver for the given sourcemap files and lets you choose the file in the UI. 

Arguments:
```

--port={port}                                                         Port to use (defaults to 8085)
--sm:{originalFile};{compiled.js};{sourcemap.js.map}       
--sm:{originalFile};{compiled.js};{sourcemap.js.map};{name}           Add the specified file. If {name} is ommited {originalFile} is used in the UI.
```

### Local installation 

```
npm install --save-dev @matthid/source-map-visualization
```


add npm script to `package.json`:

```json
{
  "scripts": {
    "check-sourcemaps": "source-map-visualization --sm=./src/fable-library/Array.fs;./build/fable-library/Array.js;./build/fable-library/Array.js.map"
    // others
  },
  // dependencies
}
```

See global installation for arguments.

Run npm

```
npm run check-sourcemaps
```


### As Library

```js
let path = require("path");
let app = require("@matthid/source-map-visualization");
let clientDir = path.resolve(__dirname, "../dist/Client");
// Start like CLI
app.startServerWithArgs(clientDir, process.argv); // argv given as specified in global installation
// Specifiy manually
let files =
  [ { name: "Array.fs", converted: "./dist/Array.js", sourceMap: "./dist/Array.js.map", original: "./src/Array.ts" } ];
app.startServer(clientDir, port, files)
```


## Install pre-requisites

You'll need to install the following pre-requisites in order to build the Source Map Visualizer

* The [.NET Core SDK](https://www.microsoft.com/net/download)
* [FAKE 5](https://fake.build/) installed as a [global tool](https://fake.build/fake-gettingstarted.html#Install-FAKE)
* The [Yarn](https://yarnpkg.com/lang/en/docs/install/) package manager (you can also use `npm` but the usage of `yarn` is encouraged).
* [Node LTS](https://nodejs.org/en/download/) installed for the front end components.
* If you're running on OSX or Linux, you'll also need to install [Mono](https://www.mono-project.com/docs/getting-started/install/).

## Build and Run (Development)

Start Server (including client)

```
yarn client-build
yarn server-build
cd src/Server
node deploy/app.<hash>.js
```

Start Client (hot module reloading, needs a started server)

```
yarn client-start
```


## Work with the application

To concurrently run the server and the client components in watch mode use the following command:

```bash
fake build -t Run
```


## SAFE Stack Documentation

You will find more documentation about the used F# components at the following places:

* [Giraffe](https://github.com/giraffe-fsharp/Giraffe/blob/master/DOCUMENTATION.md)
* [Fable](https://fable.io/docs/)
* [Elmish](https://elmish.github.io/elmish/)
* [Fulma](https://fulma.github.io/Fulma/)

If you want to know more about the full Azure Stack and all of it's components (including Azure) visit the official [SAFE documentation](https://safe-stack.github.io/docs/).

## Troubleshooting

* **fake not found** - If you fail to execute `fake` from command line after installing it as a global tool, you might need to add it to your `PATH` manually: (e.g. `export PATH="$HOME/.dotnet/tools:$PATH"` on unix) - [related GitHub issue](https://github.com/dotnet/cli/issues/9321)
