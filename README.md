# LanguageServer.NET

A .NET Language Server library for VSCode, and hopefully, might be used with other IDEs that support [MS Language Server Protocol](https://github.com/Microsoft/language-server-protocol/blob/master/protocol.md). Still work in progress and API is subject to changes.

![Screenshot of DemoLanguageServer](README.resource/Screenshot.gif)

Based on [CXuesong/JsonRpc.Standard](https://github.com/CXuesong/JsonRpc.Standard), this .NET Standard library intends to provide basic interfaces and data structures so that you can write a language server in C#, build it on .NET Core and, with the help of the client-side code of [Microsoft/vscode-languageserver-node](https://github.com/Microsoft/vscode-languageserver-node), use it in VSCode.

The library is now available on NuGet. To install the package, run the following command in the Package Manager Console

```powershell
Install-Package CXuesong.LanguageServer.VsCode -Pre
```

## To set up the demo

You may need Visual Studio 2017 to build the demo.

1.  Open `DemoLanguageServer` in VS, choose `Debug` profile, and build the project.
2.  Open `Client\VsCode` folder in VSCode.
3.  Run `npm install` in the terminal.
4.  Press F5 and a new VSCode window should show.
5.  Open a folder in the new VSCode window, and create a new file.
6.  Change the file language to `Demo Language`
7.  Then your editor will work as shown in the screenshot. Enter `.net core` in the editor and see what happens.