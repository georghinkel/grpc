// Copyright 2015 gRPC authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.IO;
using Grpc.Core;
using Helloworld;

namespace GreeterClient
{
    class Program
    {
      public static void Main(string[] args)
      {
        // if you remove the suffix 2, the following will work fine
        var cacert = File.ReadAllText(Path.Combine("..", "ca2.crt"));
        var clientcert = File.ReadAllText(Path.Combine("..", "client2.crt"));
        var clientkey = File.ReadAllText(Path.Combine("..", "client.key"));
        var ssl = new SslCredentials(cacert, new KeyCertificatePair(clientcert, clientkey), VerifyPeer);
        var channel = new Channel("localhost", 50051, ssl);
        var client = new Greeter.GreeterClient(channel);
        String user = "you";

        var reply = client.SayHello(new HelloRequest {Name = user});
        Console.WriteLine("Greeting: " + reply.Message);

        channel.ShutdownAsync().Wait();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
      }

      private static bool VerifyPeer(VerifyPeerContext context)
      {
        return true;
      }
  }
}
