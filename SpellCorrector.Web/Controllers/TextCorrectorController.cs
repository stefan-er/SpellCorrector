namespace SpellCorrector.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SpellCorrector.Web.Infrastructure;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [Produces("application/json")]
    [Route("api/TextCorrector")]
    public class TextCorrectorController : Controller
    {
        private readonly BKTree bkTree;

        public TextCorrectorController()
        {
            this.bkTree = new BKTree();

            FileStream fileStream = new FileStream("Dictionary\\Bulgarian.dic.txt", FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string dictWord = reader.ReadLine();
                while (dictWord != null)
                {
                    this.bkTree.Add(dictWord);

                    dictWord = reader.ReadLine();
                }
            }
        }

        [HttpPost]
        public JsonResult Post([FromBody]string input)
        {
            string[] inputWords = input.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

            var outputWords = new List<string>();

            foreach (string inputWord in inputWords)
            {
                string correctedWord = inputWord;
                for (int distance = 1; distance <= 4; distance++)
                {
                    List<string> suggestions = this.bkTree.Search(inputWord, distance);

                    if (suggestions.Count > 0)
                    {
                        correctedWord = suggestions.First();
                        break;
                    }
                }

                outputWords.Add(correctedWord);
            }

            string output = string.Join(' ', outputWords);

            return Json(output);
        }
    }
}
