using System;
using System.Threading;
using NBitcoin;
using NBitcoin.Protocol;


//this program is to demonstrate how to do a bitcoin transaction
//testnet address mgwASyB9G2r48Ysg2NpDPmiL4BM2UafXCc


namespace BitcoinTransaction
{
    class Program
    {
        static void Main(string[] args)
        {
            // address in storable format copied from new key below
            //Console.WriteLine(new Key().ToString(Network.TestNet));
            //this is a private key with net info included
            var secret = new BitcoinSecret
            ("cPHdfUdaC7eGg1eSYfh4A28F4CbzdaXQyMdgjoCrw6jgwAUiXtg6");//must be parsed to string
            
            //create private key from Bitcoin secret
            var privateKey = secret.PrivateKey;
            
            //create public key by combining private & public
            var publicKey = privateKey.PubKey;

            //get address from the test net using public key
            // (combines public key & network info)
            Console.WriteLine(publicKey.GetAddress(Network.TestNet));

            //create transaction
            Transaction transaction1 = new Transaction();
            var input = new TxIn();
            input.PrevOut = new OutPoint(new uint256("f0bb2a84c7521886e75e1fdb3dd4279afd8cd0b256e3b779b94af66a72562a33"),0);
            input.ScriptSig = secret.GetAddress().ScriptPubKey;
            transaction1.AddInput(input);

            //send to the recepient address
            TxOut output = new TxOut();
            var recepient = BitcoinAddress.Create("2MxCKmgUWhVGFvmxfSGonJ2v5pKhMJiPTbM");
            Money fee = Money.Satoshis(40000);
            output.Value = Money.Coins(0.65m) -fee;
            output.ScriptPubKey = recepient.ScriptPubKey;
            transaction1.AddOutput(output);
            //sign the transaction
            transaction1.Sign(secret,false);//fix this is outdated
            //Console.WriteLine(transaction1);
            
            //This command will find a testnet node
            //dig A testnet-seed.bitcoin.jonasschnelli.ch

            //propogate transaction on the network
            var node = Node.Connect(Network.TestNet,"116.62.102.29");
            node.VersionHandshake();
            node.SendMessage(new InvPayload(transaction1));
            //Send it
            node.SendMessage(new TxPayload(transaction1));
            Thread.Sleep(500); //Wait a bit
            node.Disconnect();
            
        }
    }
}
