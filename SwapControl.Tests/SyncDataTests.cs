using SwapControl.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwapControl.Structure.Configs.Factories;
using SwapControl.Structure.Configs.Interfaces;
using SwapControl.SQL.Interface;
using SwapControl.SQL;
using SwapControl.SQL.Entities;
using SwapControl.MT;

namespace SwapControl.Tests
{
    public class SyncDataTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestInitSQLWrapper()
        {
            IPostgreSQL appDbContext = new AppDbContext("host=localhost;port=5432;database=MT4;username=postgres;password=1234");
            
            SQLWrapper sqlWrapper = new SQLWrapper(appDbContext);

            Assert.IsInstanceOf<SQLWrapper>(sqlWrapper);
        }

        [Test]
        public void TestIPostgreSQLConnection()
        {
            IPostgreSQL appDbContext = new AppDbContext("host=localhost;port=5432;database=MT4;username=postgres;password=1234");
           
            Assert.IsInstanceOf<AppDbContext>(appDbContext);

            Assert.AreEqual(true, appDbContext.isConnected(), "There may be an error in the DB connection string");
        }

        [Test]
        public void TestSQLGetSymbols()
        {
            IPostgreSQL appDbContext = new AppDbContext("host=localhost;port=5432;database=MT4;username=postgres;password=1234");
            SQLWrapper sqlWrapper = new SQLWrapper(appDbContext);
            List<Symbol> symbolsSQL = sqlWrapper.GetSymbols();

            Assert.IsInstanceOf<List<Symbol>>(symbolsSQL);
            Assert.Greater(symbolsSQL.Count, 0);
        }



    }
}
