using System;
using LanguageServer.VsCode.JsonRpc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static UnitTestProject1.Utility;

namespace UnitTestProject1
{
    [TestClass]
    public class JsonRpcTests
    {
        [TestMethod]
        public void MessageReaderTest1()
        {
            const string source =
                "Content-Length: 82\r\n\r\n{\"jsonrpc\": \"2.0\",\"id\": 1,\"method\": \"textDocument/didOpen\",\"params\": {\"A\":\"test\"}}";
            using (var ss = StringToStream(source))
            {
                var reader = new StreamMessageReader(ss);
                var message = (RequestMessage) reader.Read();
                Assert.AreEqual("2.0", message.Version);
                Assert.AreEqual(1, message.Id);
                Assert.AreEqual("textDocument/didOpen", message.Method);
                Assert.AreEqual("test", message.Params["A"]);
            }
        }
    }
}
