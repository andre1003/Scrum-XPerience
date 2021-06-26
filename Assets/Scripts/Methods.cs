using System.Collections;
using System.Collections.Generic;

namespace Methods {
    public class Methododology {
        protected string method;
        protected string methodDescription;
        protected List<string> functions;
        protected Dictionary<string, string> functionDescription;
        protected Dictionary<string, List<string>> choices;

        /*public Methododology(string method, List<string> functions, Dictionary<string, List<string>> choices) {
            this.method = method;
            this.functions = functions;
            this.choices = choices;
        }*/

        public string GetMethod() {
            return this.method;
        }

        public string GetMethodDescription() {
            return this.methodDescription;
        }

        public string GetFunctionDescription(string function) {
            return functionDescription[function];
        }

        public List<string> GetFunctions() {
            return functions;
        }

        public List<string> GetChoicesByKey(string key) {
            return this.choices[key];
        }
    }

    public class Scrum : Methododology {
        public Scrum() {
            this.method = "Scrum";

            this.methodDescription = "O desenvolvimento utilizando a metodologia Scrum é feito em incrementos, os quais são chamados de Sprint, e levam, em média, de duas a quatro semanas. Ao final de cada Sprint, ocorre as etapas de avaliação do incremento implementado e o modo como isso ocorreu, denominadas, respectivamente, Revisão da Sprint e Retrospectiva da Sprint. Além disso, o Time Scrum possui três entidades, sendo elas: Product Owner, Scrum Master e Time de Desenvolvimento.";

            this.functions = new List<string>() {
                 "Scrum Master",
                 "Product Owner",
                 "Development Team"
            };

            List<string> scrumMaster = new List<string>() {
                "Daily Scrum",
                "Get Requirement",
                "Produce Spring Backlog",
                "Check Deadline and Costs"
            };



            List<string> productOwner = new List<string>() {
                "Daily Scrum",
                "Talk with Client",
                "Produce Product Backlog",
                "Valorize the Product"
            };

            List<string> devTeam = new List<string>() {
                "Awnser Daily Scrum Questions",
                "Do Sprint Planning",
                "Implement Spring Backlog Requirements",
                "Update Deadline"
            };


            this.choices = new Dictionary<string, List<string>>() {
                { "Scrum Master", scrumMaster },
                { "Product Owner", productOwner },
                { "Development Team", devTeam }
            };

            this.functionDescription = new Dictionary<string, string>() {
                { "Scrum Master", "You are about to be Scrum Master" },
                { "Product Owner", "You are about to be Product Owner" },
                { "Development Team", "You are about to be Development Team Member" }
            };
        }
    }

    public class XP : Methododology {
        public XP() {
            this.method = "EXtreme Programming";

            this.methodDescription = "This is EXtreme Programming, or most called XP, an agile method for software development";

            this.functions = new List<string>() {
                "Software Manager",
                "Test Engineer",
                "Developer"
            };

            List<string> softwareManager = new List<string>() {
                "Talk with Client",
                "Help Developers Follow XP Concepts",
                "Get Requirements",
                "Determinate Deadline and Costs"
            };

            List<string> testEngineer = new List<string>() {
                "Talk with Client",
                "Project Tests Before the Development",
                "Test the Software",
                "Help Update Deadline and Costs"
            };

            List<string> developer = new List<string>() {
                "Check Requirements to Make an Increment Planning",
                "Do Meeting with Team",
                "Implement Increment Requirements",
                "Check Deadline and Costs"
            };

            this.choices = new Dictionary<string, List<string>>() {
                { "Software Manager", softwareManager },
                { "Test Engineer", testEngineer },
                { "Developer", developer }
            };

            this.functionDescription = new Dictionary<string, string>() {
                { "Software Manager", "You are about to be Software Manager" },
                { "Test Engineer", "You are about to be Test Engineer" },
                { "Developer", "You are about to be Developer" }
            };
        }
    }
}
