using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using TailSpin.SpaceGame.Web;
using TailSpin.SpaceGame.Web.Models;

namespace Tests
{
    public class DocumentDBRepository_ReturnRequestedCount 
    {
        private IDocumentDBRepository<Score>  _scoreRepository;

        [SetUp]
        public void Setup() 
        {
            using(Stream s = typeof(IDocumentDBRepository<Score>).Assembly.GetManifestResourceStream("Tailspin.SpaceGame.Web.SampleData.scores.json")) 
            {
                _scoreRepository = new LocalDocumentDBRepository<Score>(s);
            }
        }

        [TestCase(0,ExpectedResult=0)]
        [TestCase(1,ExpectedResult=1)]
        [TestCase(10,ExpectedResult=10)]
        public int ReturnRequestedCount(int inputCount) 
        {
            const int PAGE=0;

            //Fetch scores
            Task<IEnumerable<Score>> scoresTask = _scoreRepository.GetItemsAsync(
                score => true,  //return all scores
                score => 1,     //we don't care about the order
                PAGE,           //take first page of results
                inputCount      //fetch this number of results
            );
            IEnumerable<Score> scores = scoresTask.Result;

            //Assert/Verify we received the correct number of items in the collection
            //NOTE: removed the need to explicitly call the assertion cuz test method uses the "ExpectedResult" property to simplify code and help make intenion clear. Nunit will auto compare return function value against the value of ExpectedResult property.
            return scores.Count();
        }
    }
}