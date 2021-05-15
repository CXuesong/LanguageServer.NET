import * as fs from "fs";
import * as path from "path";
import * as vscode from "vscode";
import { LanguageClient, LanguageClientOptions, ServerOptions } from "vscode-languageclient/node";

// Defines the search path of your language server DLL. (.NET Core)
const languageServerPaths = [
    "server/DemoLanguageServer.dll",
    "../../DemoLanguageServer/bin/Debug/netcoreapp3.1/DemoLanguageServer.dll",
]

let client: LanguageClient | undefined;

async function activateLanguageServer(context: vscode.ExtensionContext) {
    // The server is implemented in an executable application.
    let serverModule: string | undefined;
    for (let p of languageServerPaths) {
        p = context.asAbsolutePath(p);
        console.log(p);
        try {
            await fs.promises.access(p);
            serverModule = p;
            break;
        } catch (err) {
            // Skip this path.
        }
    }
    if (!serverModule) throw new URIError("Cannot find the language server module.");
    let workPath = path.dirname(serverModule);
    console.log(`Use ${serverModule} as server module.`);
    console.log(`Work path: ${workPath}.`);


    // If the extension is launched in debug mode then the debug server options are used
    // Otherwise the run options are used
    let serverOptions: ServerOptions = {
        run: { command: "dotnet", args: [serverModule], options: { cwd: workPath } },
        debug: { command: "dotnet", args: [serverModule, "--debug"], options: { cwd: workPath } }
    }
    // Options to control the language client
    let clientOptions: LanguageClientOptions = {
        // Register the server for plain text documents
        documentSelector: ["demolang"],
        synchronize: {
            // Synchronize the setting section 'languageServerExample' to the server
            configurationSection: "demoLanguageServer",
            // Notify the server about file changes to '.clientrc files contain in the workspace
            fileEvents: [
                vscode.workspace.createFileSystemWatcher("**/.clientrc"),
                vscode.workspace.createFileSystemWatcher("**/.demo"),
            ]
        },
    }

    // Create the language client and start the client.
    client = new LanguageClient("demoLanguageServer", "Demo Language Server", serverOptions, clientOptions);
    let disposable = client.start();

    // Push the disposable to the context's subscriptions so that the 
    // client can be deactivated on extension deactivation
    context.subscriptions.push(disposable);
}

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export async function activate(context: vscode.ExtensionContext) {
    console.log("demolang extension is now activated.");

    await activateLanguageServer(context);

    // The command has been defined in the package.json file
    let disposable = vscode.commands.registerCommand("extension.sayHello", () => {
        // The code you place here will be executed every time your command is executed

        // Display a message box to the user
        vscode.window.showInformationMessage("Hello World!");
    });

    context.subscriptions.push(disposable);
}

// this method is called when your extension is deactivated
export async function deactivate(): Promise<void> {
    client?.stop();
}