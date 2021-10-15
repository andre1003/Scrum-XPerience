using System.Collections;
using System.Collections.Generic;

namespace Methods {
    public class Methododology {
        protected string method;
        protected string methodDescription;
        protected List<string> functions;
        protected Dictionary<string, string> functionDescription;

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

            this.functionDescription = new Dictionary<string, string>() {
                { "Scrum Master", "Salas de escolhas: Sala de Reunião com Equipe e Sala de Reunião Cliente.\n\nDescrição: O papel do Scrum Master é assegurar que o Scrum seja compreendido e executado por toda a equipe de desenvolvimento. Ele também é o responsável por determinar os limites da Sprint, em conjunto com o Product Owner. Além disso, Scrum Master é o responsável pela condução e organização da daily scrum. Desse modo, o Scrum Master tem suas escolhas voltadas para a organização dos requisitos do software (por prioridade de desenvolvimento), juntamente com o Product Owner. Além disso, ele deverá atuar na reunião com a equipe, além de poder ter acesso às estatísticas do grupo (que são mostradas na sala de reunião com a diretoria)." },
                { "Product Owner", "Salas de escolhas: Sala de Reunião com Equipe e Sala de Reunião com Cliente.\n\nDescrição: As principais funções do Product Owner são: definir os itens do Backlog de Produto; realizar a priorização desses itens, possibilitando a realização do Sprint Backlog; e garantir que não haja interferência nos requisitos que estão sendo implementados em uma determinada Sprint. Desse modo, o Product Owner tem suas escolhas relacionadas à licitação dos requisitos que o software fictício deve conter, organizando-os de acordo com a prioridade de desenvolvimento. Além disso, ele irá auxiliar a definir o escopo do desenvolvimento de uma Sprint. Por fim, o Product Owner pode verificar as estatísticas do jogo, junto com o Scrum Master." },
                { "Development Team", "Salas de escolhas: Sala de Reunião com Equipe e Sala de Desenvolvimento.\n\nDescrição: O Time de Desenvolvimento Scrum é a parte da equipe que irá desenvolver o software de fato. Assim, eles possuem conhecimento técnico necessário para realizar todas as etapas da fase de produção do sistema, desde a prototipagem até a entrega do produto final, passando por revisões de código e elaboração de documentações. Desse modo, no jogo, o jogador que escolher esse papel vai representar apenas um membro do time de desenvolvimento. Assim, ele deve fazer escolhas dentro do escopo do time de desenvolvimento, as quais se referem, em sua maior parte, ao desenvolvimento do software fictício. Além disso, o membro do time de desenvolvimento deve participar da reunião da equipe, para participar das reuniões diárias, planejar, revisar e fazer a retrospectiva da Sprint." }
            };
        }
    }

    public class XP : Methododology {
        public XP() {
            this.method = "EXtreme Programming";

            this.methodDescription = "O desenvolvimento utilizando a metodologia ágil eXtreme Programming (XP) é feito de forma incremental, entregando ao cliente suas necessidades imediatas. Além disso, a XP aprimora o desenvolvimento do software aplicando cinco valores: constante comunicação entre clientes e programadores; manter o software o mais simples possível; obter feedback por meio de testes; a cada sucesso, aumenta-se o respeito entre os membros da equipe; e, ao aplicar todos os demais valores, a equipe desenvolve coragem para responder a mudanças.";

            this.functions = new List<string>() {
                "Software Manager",
                "Test Engineer",
                "Developer"
            };

            this.functionDescription = new Dictionary<string, string>() {
                { "Software Manager", "Salas de escolhas: Sala de Reunião com Equipe e Sala de Reunião Cliente.\n\nDescrição: O gerente de projetos é o responsável por gerenciar o desenvolvimento do software, garantindo que o sistema seja desenvolvido de acordo com o desejo do cliente. O aluno que escolher este papel deve se responsabilizar por gerenciar o desenvolvimento do projeto fictício, conduzindo escolhas de forma semelhante a uma junção de Product Owner com Scrum Master." },
                { "Test Engineer", "Salas de escolhas: Sala de Reunião com Equipe e Sala de Desenvolvimento.\n\nDescrição: Sem dúvidas um papel crucial para a XP, o Engenheiro de Testes é o responsável pelos seguintes aspectos: definir quais testes serão aplicados no software a ser desenvolvido; auxiliar na elaboração desses testes (produzindo testes automatizados com as devidas ferramentas); e analisar os resultados dos testes aplicados na iteração desenvolvida, viabilizando ou não seu release. Assim, o aluno que escolher esse papel fica responsável pela parte mais importante do desenvolvimento segundo a metodologia XP: os testes. As escolhas dele certamente terão grande impacto na condução correta ou não dos valores da XP, o que impacta no desenvolvimento do software." },
                { "Developer", "Salas de escolhas: Sala de Reunião com Equipe e Sala de Desenvolvimento.\n\nDescrição: O desenvolvedor é o responsável pela elaboração dos incrementos e, consequentemente, do software final a ser entregue. Desse modo, ele deve trabalhar em uma equipe auto organizável, de modo a realizarem uma seleção dos requisitos a serem implementados por incremento. O principal ponto é a entrega frequente de releases, construindo o software de “pouco em pouco”. Assim, o jogador que escolher esse papel terá basicamente as mesmas escolhas do membro do time de desenvolvimento Scrum, apenas adequando os nomes e algumas ações para a metodologia XP." }
            };
        }
    }
}
