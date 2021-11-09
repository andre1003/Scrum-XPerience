using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Net.Http;

public class FeedbackManager : MonoBehaviour {
    public Text feedbackText;
    //public ErrorManager errorManager;
    public GameObject continueButton;
    public GameObject continueButton2;

    public Text titleText;
    public Text individualMistakes;
    public Text individualHits;

    public GameObject feedbackCanvas;
    public GameObject mistakesAndHitsCanvas;
    public GameObject endCanvas;

    public InputField passwordInputField;
    public GameObject passwordCanvas;
    public Text infoText;

    public Toggle formsToggle;

    private string mistakeFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\mistakes.txt";
    private string individualFeedbackFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\individual_feedback.txt";
    private string[] lines;

    private string timeoutText = "Você demorou muito para tomar as decisões! Mais atenção!";
    private string teamMeetingText = "Sua comunicação com a equipe foi falha! Muito cuidado, pois essa é uma etapa fundamental para o desenvolvimento ágil.";
    private string clientMeetingText = "A comunicação com o cliente é essencial para os métodos ágeis! Lembre-se sempre disso...";
    private string developmentRoomText = "Apesar de tudo, o desenvolvimento do software não foi dos melhores... Busque estudar mais sobre o desenvolvimento utilizando metodologias ágeis. Isso vai te ajudar bastante!";

    private string individualFeedback = "";
    private string generalFeedback = "";

    private string saveMatchExecutablePath = Directory.GetCurrentDirectory() + @"\Assets\Scripts\send_to_cloud.exe";
    private string decisionsPath;

    private string password;

    private List<string> feedback = new List<string>();

    private bool canContinue = false;

    private short count = 0;

    private string path;

    private int timeouts = 0;
    private int teamMeetingMistakes = 0;
    private int clientMeetingMistakes = 0;
    private int developmentMistakes = 0;
    private int teamMeetingHits = 0;
    private int clientMeetingHits = 0;
    private int developmentHits = 0;

    private string individualStats;

    private void Awake() {
        path = Application.persistentDataPath + "/player_data/";
    }

    // Start is called before the first frame update
    void Start() {
        decisionsPath = Application.persistentDataPath + "/player_data/";
        passwordCanvas.SetActive(true);
        individualStats = GetIndividualFeedback();
    }

    private void Update() {
        if(canContinue) {
            if(Input.GetKeyDown(KeyCode.Return)) {
                if(count == 1)
                    ShowIndividualHitsAndMistakes();
                else if(count == 3)
                    ShowIndividualFeedback();
                else if(count == 4)
                    ShowGeneralStats();
                else if(count == 5)
                    ShowGeneralHitsAndMistakes();
                else if(count == 7)
                    End();
            }
        }
    }

    private string GetIndividualFeedback() {
        List<int> mistakes = GetIndividualStats();
        // 1 - Team Meeting
        // 2 - Client Meeting
        // 3 - Development Room

        string role = PlayerPrefs.GetString("player_function");

        if(role.Equals("Desenvolvedor")) {
            if(mistakes[1] <= 4)
                individualFeedback += "Como Desenvolvedor, você relatou bem o desenvolvimento do software, além de ter ajudado no planejamento do incremento. Sua comunicação com os membros da equipe foi muito boa. Parabéns!\n\n";
            else if(mistakes[1] <= 10)
                individualFeedback += "Lembre-se que, como Desenvolvedor, você deve sempre comunicar à equipe o status de desenvolvimento do software, identificando problemas e planejando o desenvolvimento, além de auxiliar no planejamento do incremento. Você cumpriu alguns desses pontos, mas pecou em outros. Portanto, você pode melhorar um pouco mais sua comunicação com a equipe. No geral ela não foi ruim, mas requer sua atenção nos estudos!\n\n";
            else if(mistakes[1] > 10)
                individualFeedback += "Como Desenvolvedor, na Sala de Reunião com a Equipe, você deve sempre planejar o desenvolvimento do incremento, comunicar o que foi desenvolvido e os problemas encontrados e revisar tanto a implementação quanto o modo que o incremento foi conduzido. Assim, sua comunicação com a equipe foi falha. Reforce seus estudos a respeito desse tópico.\n\n";


            if(mistakes[3] <= 4)
                individualFeedback += "Como Desenvolvedor na Sala de Desenvolvimento, você implementou corretamente o sistema de acordo com os princípios da XP, como realizar testes, programar em pares e realizar integração contínua. Continue assim!\n\n";
            else if(mistakes[3] <= 10)
                individualFeedback += "Como Desenvolvedor na Sala de Desenvolvimento, você deve se preocupar com as atividades relacionadas à implementação, como realizar testes no sistema, realizar o desenvolvimento em pares, realizar integração contínua, etc. Assim, o software foi desenvolvido de forma correta, mas não do melhor modo possível. Lembre-se sempre que é importante implementar os requisitos por prioridades, realizar testes, refatorar e documentar código e integrar.\n\n";
            else if(mistakes[3] > 10)
                individualFeedback += "Como Desenvolvedor na Sala de Desenvolvimento, você deveria se preocupar em realizar testes no sistema, realizar o desenvolvimento em pares, realizar integração contínua, corrigir problemas identificados, entre outras atividades de implementação. Assim, o desenvolvimento do software foi falho. É muito importante que você volte seus estudos para as principais etapas do desenvolvimento, como implementação por prioridade, refatoração, documentação de código, integração e testes, por exemplo.\n\n";
        }
        else if(role.Equals("Engenheiro de Testes")) {
            if(mistakes[1] <= 4)
                individualFeedback += "Como Engenheiro de Testes na Sala de Reunião com a Equipe, você focou em identificar os testes, comunicar seus resultados, além de auxiliar no planejamento, tanto do dia quanto do incremento. Dessa forma, sua comunicação com os membros da equipe foi muito boa. Parabéns!\n\n";
            else if(mistakes[1] <= 10)
                individualFeedback += "Como Engenheiro de Testes na Sala de Reunião com a Equipe, o ideal seria manter o foco nos testes, porém sem deixar de auxiliar no planejamento do desenvolvimento. Com isso, você pode melhorar um pouco mais sua comunicação com a equipe. No geral ela não foi ruim, mas requer sua atenção nos estudos!\n\n";
            else if(mistakes[1] > 10)
                individualFeedback += "Como Engenheiro de Testes na Sala de Reunião com Equipe, você deve sempre se preocupar com os testes que foram e vão ser realizados no sistema, mas não deixando de ajudar o restante da equipe no planejamento da implementação. Portanto, sua comunicação com a equipe foi falha. Reforce seus estudos a respeito desse tópico.\n\n";


            if(mistakes[3] <= 4)
                individualFeedback += "O Engenheiro de Testes na Sala de Desenvolvimento deve sempre se preocupar em definir, elaborar e aplicar os testes do sistema, verificando seus resultados e apontando melhorias e correções, como você fez. Assim, você conduziu bem o desenvolvimento do sistema. Continue assim!\n\n";
            else if(mistakes[3] <= 10)
                individualFeedback += "Você, como Engenheiro de Testes na Sala de Desenvolvimento, precisa definir, elaborar, automatizar e aplicar os testes do sistema, além de verificar seus resultados, identificando correções e aprimoramentos. Você cumpriu esses aspectos parcialmente. Assim, o software foi desenvolvido de forma correta, mas não do melhor modo possível. Lembre-se sempre que é importante implementar os requisitos por prioridades, realizar testes, refatorar e documentar o código e integrar.\n\n";
            else if(mistakes[3] > 10)
                individualFeedback += "Você, como Engenheiro de Testes na Sala de Desenvolvimento, deveria ter focado nos testes do sistema, elaborando e aplicando, de modo a verificar seus resultados e apontar melhorias e correções. Desse modo, com base nas suas decisões, o desenvolvimento do software foi falho. É muito importante que você volte seus estudos para as principais etapas do desenvolvimento, como implementação por prioridade, refatoração, documentação de código, integração e testes, por exemplo.\n\n";
        }
        else if(role.Equals("Gerente de Projetos")) {
            if(mistakes[1] <= 4)
                individualFeedback += "Como Gerente de Projetos na Sala de Reunião com a Equipe, você discutiu os feedbacks do cliente, além de apresentar os requisitos definidos por você e auxiliar a equipe no planejamento do incremento e do dia. Desse modo, sua comunicação com os membros da equipe foi muito boa. Parabéns!\n\n";
            else if(mistakes[1] <= 10)
                individualFeedback += "Você, como Gerente de Projetos, na Sala de Reunião com a Equipe, deveria voltar sua participação para apontar os requisitos definidos, de modo a planejar em conjunto o desenvolvimento do incremento, além de identificar os problemas no software e no andamento do desenvolvimento, sugerindo soluções. Com isso, você pode melhorar um pouco mais sua comunicação com a equipe. No geral ela não foi ruim, mas requer sua atenção nos estudos!\n\n";
            else if(mistakes[1] > 10)
                individualFeedback += "O Gerente de Projetos, na Sala de Reunião com Equipe, deve se dedicar em auxiliar no planejamento do incremento, bem como na verificação dos problemas existentes e na discussão do feedback do cliente. A partir disso, conclui-se que sua comunicação com a equipe foi falha. Reforce seus estudos a respeito desse tópico.\n\n";


            if(mistakes[2] <= 4)
                individualFeedback += "Você como Gerente de Projetos na Sala de Reunião com o Cliente se comportou adequadamente, levantando e atualizando os requisitos do sistema, organizando-os por prioridade, além de trazer o cliente ao local sempre que possível. Desse modo, você se comunicou muito bem com o cliente. Muito bem!\n\n";
            else if(mistakes[2] <= 10)
                individualFeedback += "Você, como Gerente de Projetos, na Sala de Reunião com o Cliente, deveria se atentar mais ao processo de Engenharia de Requisitos, além de trazer o cliente ao local de desenvolvimento sempre que possível. Assim, sua comunicação com o cliente não foi das melhores... Não é ruim, mas pode melhorar.\n\n";
            else if(mistakes[2] > 10)
                individualFeedback += "O Gerente de Projetos, na Sala de Reunião com o Cliente, deve sempre realizar a Engenharia de Requisitos, definindo as funcionalidades e as ordenando por prioridade, além de convidar o cliente a ir ao ambiente de desenvolvimento do software. Com isso, baseado em suas escolhas, sua comunicação com o cliente foi ruim. Lembre-se de estudar mais a respeito disso!\n\n";
        }
        else if(role.Equals("Scrum Master")) {
            if(mistakes[1] <= 4)
                individualFeedback += "Você, como Scrum Master, na Sala de Reunião com a Equipe, auxiliou com o planejamento das Sprints, além de conduzir as daily scrums e garantir que o Scrum fosse corretamente aplicado. Desse modo, sua comunicação com os membros da equipe foi muito boa. Parabéns!\n\n";
            else if(mistakes[1] <= 10)
                individualFeedback += "Lembre-se que o Scrum Master, na Reunião com a Equipe, deve sempre auxiliar no entendimento do Scrum por todos os demais membros, além de ajudar no planejamento, revisão e retrospectiva da Sprint. Assim, você pode melhorar um pouco mais sua comunicação com a equipe. No geral ela não foi ruim, mas requer sua atenção nos estudos!\n\n";
            else if(mistakes[1] > 10)
                individualFeedback += "O Scrum Master deve sempre garantir a correta aplicação dos conceitos e valores do Scrum pelos membros da equipe, além de auxiliar no planejamento, revisão e retrospectiva da Sprint. Com base nas suas decisões, conclui-se que sua comunicação com a equipe foi falha. Portanto, reforce seus estudos a respeito desse tópico.\n\n";


            if(mistakes[2] <= 4)
                individualFeedback += "Você, como Scrum Master, na Sala de Reunião com o Cliente, auxiliou bastante o Product Owner, revisando os requisitos e a ordem de prioridade estabelecida, além de ajudar na entrega dos incrementos, coletando o feedback do cliente. Assim, você se comunicou muito bem com o cliente e o trouxe para o desenvolvimento sempre que possível. Muito bem!\n\n";
            else if(mistakes[2] <= 10)
                individualFeedback += "É importante que você se lembre que o papel do Scrum Master na Sala de Reunião com o Cliente é auxiliar ao máximo o Product Owner, revisando as funcionalidades identificadas, na entrega do incremento e trazendo o cliente para o desenvolvimento. Desse modo, sua comunicação com o cliente não foi das melhores... Não é ruim, mas pode melhorar.\n\n";
            else if(mistakes[2] > 10)
                individualFeedback += "O Scrum Master, na Sala de Reunião com o Cliente, deve se dispor a ajudar o Product Owner o máximo possível. Para isso, ele pode revisar os requisitos, bem como a prioridade estabelecida, convidar o cliente para o ambiente de desenvolvimento e auxiliar nas entregas dos incrementos, de modo a coletar seu feedback. A partir disso e tomando como base suas escolhas, conclui-se que sua comunicação com o cliente foi ruim. Lembre-se de estudar mais a respeito disso!\n\n";
        }
        else if(role.Equals("Product Owner")) {
            if(mistakes[1] <= 4)
                individualFeedback += "Você, como Product Owner, na Sala de Reunião com a Equipe, apresentou o Backlog do Produto, auxiliando na definição dos Backlogs das Sprints, além de auxiliar o planejamento das Sprints, identificar problemas e realizar a revisão e retrospectiva das Sprint. Desse modo, sua comunicação com os membros da equipe foi muito boa. Parabéns!\n\n";
            else if(mistakes[1] <= 10)
                individualFeedback += "Como Product Owner, na Sala de Reunião com a Equipe, você poderia auxiliar no planejamento das Sprints, bem como participar nas daily scrums e na revisão e retrospectiva das Sprints. Você contemplou esses aspectos de forma parcial, porém você pode melhorar um pouco mais sua comunicação com a equipe. No geral ela não foi ruim, mas requer sua atenção nos estudos!\n\n";
            else if(mistakes[1] > 10)
                individualFeedback += "O Product Owner, na Sala de Reunião com a Equipe, deve sempre se atentar em discutir e ajudar a definir os Backlogs das Sprints, além de participar das daily scrums e revisão e retrospectiva das Sprints. Assim, sua comunicação com a equipe foi falha. Reforce seus estudos a respeito desse tópico.\n\n";


            if(mistakes[2] <= 4)
                individualFeedback += "Você, como Product Owner, na Sala de Reunião com Cliente, levantou adequadamente os requisitos, ordenando-os por prioridades, além de convidar o cliente para o ambiente de desenvolvimento sempre que possível. Assim, você se comunicou muito bem com o cliente e o trouxe para o desenvolvimento sempre que possível. Muito bem!\n\n";
            else if(mistakes[2] <= 10)
                individualFeedback += "Como Product Owner, na Sala de Reunião com o Cliente, o ideal é conduzir a Engenharia de Requisitos, ordenando as funcionalidades pelas prioridades, trazer o cliente para o desenvolvimento e observar seus feedbacks sobre o sistema. Desse modo, sua comunicação com o cliente não foi das melhores... Não é ruim, mas pode melhorar.\n\n";
            else if(mistakes[2] > 10)
                individualFeedback += "O Product Owner, na Sala de Reunião com o Cliente, deve sempre realizar o levantamento dos requisitos, de modo a produzir o Backlog do Produto, além de trazer o cliente para o ambiente de desenvolvimento e observar suas opiniões do sistema. Assim, com base em suas decisões, sua comunicação com o cliente foi ruim. Lembre-se de estudar mais a respeito disso!\n\n";
        }
        else { // Development Team
            if(mistakes[1] <= 4)
                individualFeedback += "Como membro do Time de Desenvolvimento, na Sala de Reunião com a Equipe, você relatou bem o desenvolvimento do software, além de ter ajudado no planejamento da Sprint e participado das daily scrums. Sua comunicação com os membros da equipe foi muito boa. Parabéns!\n\n";
            else if(mistakes[1] <= 10)
                individualFeedback += "Lembre-se que, como membro do Time de Desenvolvimento, na Sala de Reunião com a Equipe, você deve sempre comunicar à equipe o status de desenvolvimento do software, identificando problemas e realizando daily scrums, além de auxiliar no planejamento da Sprint e na revisão e retrospectiva das Sprints. Você cumpriu alguns desses pontos, mas pecou em outros. Portanto, você pode melhorar um pouco mais sua comunicação com a equipe. No geral ela não foi ruim, mas requer sua atenção nos estudos!\n\n";
            else if(mistakes[1] > 10)
                individualFeedback += "Como membro do Time de Desenvolvimento, na Sala de Reunião com a Equipe, você deve sempre planejar a Sprint, comunicar o que foi desenvolvido e os problemas encontrados e revisar tanto a implementação quanto o modo que o incremento foi conduzido. Assim, sua comunicação com a equipe foi falha. Reforce seus estudos a respeito desse tópico.\n\n";


            if(mistakes[3] <= 4)
                individualFeedback += "Você, como membro do Time de Desenvolvimento, conduziu a implementação do software seguindo o Backlog da Sprint, realizando os devidos testes e corrigindo os erros encontrados. Isso significa que você conduziu bem o desenvolvimento do sistema. Continue assim!\n\n";
            else if(mistakes[3] <= 10)
                individualFeedback += "Você, que fez parte do Time de Desenvolvimento, na Sala de Desenvolvimento, deveria voltar sua atenção para implementar o software, seguindo a ordem dos Backlogs da Sprint, realizando testes no sistema e corrigindo problemas. Com isso, o software foi desenvolvido de forma correta, mas não do melhor modo possível. Lembre-se sempre que é importante implementar os requisitos por prioridades, realizar testes, refatorar e documentar código e integrar.\n\n";
            else if(mistakes[3] > 10)
                individualFeedback += "O membro do Time de Desenvolvimento, na Sala de Desenvolvimento, deve sempre implementar os requisitos de acordo com os Backlogs das Sprints, além de realizar os devidos testes, correções, aprimoramentos e integrações no sistema. Portanto, de acordo com suas escolhas, o desenvolvimento do software foi falho. É muito importante que você volte seus estudos para as principais etapas do desenvolvimento, como implementação por prioridade, refatoração, documentação de código, integração e testes, por exemplo.\n\n";
        }
        

        // No mistakes
        if(mistakes[0] == 0 && mistakes[1] == 0 && mistakes[2] == 0 && mistakes[3] == 0)
            individualFeedback += "Parabéns. Você não errou nenhuma vez!";

        return "Erros em Reuniao de Equipe: " + mistakes[1] + "\nErros em Reuniao com Cliente: " + mistakes[2] + "\nErros de Desenvolvimento: " + mistakes[3];
    }

    public void Login() {
        password = passwordInputField.text;
        SendDataToDatabase();
    }

    private void SendDataToDatabase() {
        string username = PlayerPrefs.GetString("username");
        string role = PlayerPrefs.GetString("player_function");
        string group = PlayerPrefs.GetString("group");
        //string username = "andre.aragao";
        //string role = "Scrum Master";
        //string group = "Tigers";

        List<Decision> decisions = SaveSystem.LoadAll();
        int hits = 0;
        int mistakes = 0;
        foreach(Decision decision in decisions) {
            if(decision.isMistake == true)
                mistakes++;
            else
                hits++;
        }

        SaveSystem.InfoFile("starting", username, role, hits.ToString(), mistakes.ToString(), individualFeedback, group, "");

        HttpClient client = new HttpClient();
        Dictionary<string, string> data = new Dictionary<string, string>();
        
        // Login
        data.Add("username", username);
        data.Add("password", password);
        string response = SaveSystem.Post(SaveSystem.loginUrl, data, client);
        data.Clear();

        if(response.Equals("fail")) {
            infoText.color = Color.red;
            infoText.text = "Senha incorreta! Tente novamente.";
        }
        else if(!response.Equals("Connection Failed")) {
            passwordCanvas.SetActive(false);

            // Save match
            data.Add("role", role);
            data.Add("hits", hits.ToString());
            data.Add("mistakes", mistakes.ToString());
            data.Add("individual_feedback", individualFeedback);
            data.Add("group", group);
            string matchId = SaveSystem.Post(SaveSystem.matchRegisterUrl, data, client);

            if(!matchId.Equals("Connection Failed")) {
                SaveSystem.InfoFile("match created", username, role, hits.ToString(), mistakes.ToString(), individualFeedback, group, matchId);
                data.Clear();

                // Save decisions
                foreach(Decision decision in decisions) {
                    data.Add("decision", decision.decisionId);
                    data.Add("scenery", decision.scenery);
                    data.Add("is_mistake", decision.isMistake.ToString());
                    SaveSystem.Post(SaveSystem.decisionRegisterUrl + matchId + "/", data, client);
                    data.Clear();
                }

                SaveSystem.InfoFile("done", username, role, hits.ToString(), mistakes.ToString(), individualFeedback, group, matchId);
                
            }
            ShowIndividualStats(individualStats);
        }
        else {
            UnityEngine.Debug.Log(response);
            ShowIndividualStats(individualStats);
        }

        
    }

    public void ShowIndividualStats(string stats) {
        titleText.text = "Estatísticas Individuais";
        canContinue = false;
        StartCoroutine(TypewriterEffect(stats, feedbackText));
    }

    private void ShowIndividualHitsAndMistakes() {
        canContinue = false;
        mistakesAndHitsCanvas.SetActive(true);
        continueButton.SetActive(false);
        continueButton2.SetActive(false);
        ShowIndividualMistakes();
        ShowIndividualHits();
    }

    public void ShowIndividualMistakes() {
        List<string> mistakes = GetMistakes();
        string aux = "";
        foreach(string mistake in mistakes) {
            aux += mistake + "\n";
        }

        StartCoroutine(TypewriterEffect(aux, individualMistakes));
    }

    public void ShowIndividualHits() {
        List<string> hits = GetHits();
        string aux = "";
        foreach(string hit in hits) {
            aux += hit + "\n";
        }

        StartCoroutine(TypewriterEffect(aux, individualHits));
    }

    private void ShowIndividualFeedback() {
        titleText.text = "Feedback Individual";
        canContinue = false;
        continueButton.SetActive(false);
        continueButton2.SetActive(false);
        feedbackText.text = "";
        mistakesAndHitsCanvas.SetActive(false);

        StartCoroutine(TypewriterEffect(individualFeedback, feedbackText));
    }

    private void ShowGeneralStats() {
        titleText.text = "Estatísticas do Grupo";
        canContinue = false;
        continueButton.SetActive(false);
        continueButton2.SetActive(false);
        feedbackText.text = "";
        mistakesAndHitsCanvas.SetActive(false);
        List<int> mistakes = GetGeneralStats();
        string stats = "Erros em Reuniao de Equipe: " + mistakes[1] + "\nErros em Reuniao com Cliente: " + mistakes[2] + "\nErros de Desenvolvimento: " + mistakes[3];
        StartCoroutine(TypewriterEffect(stats, feedbackText));


    }

    private void ShowGeneralHitsAndMistakes() {
        canContinue = false;
        mistakesAndHitsCanvas.SetActive(true);
        continueButton.SetActive(false);
        continueButton2.SetActive(false);
        individualMistakes.text = "";
        individualHits.text = "";
        ShowGeneralMistakes();
        ShowGeneralHits();
    }

    public void ShowGeneralMistakes() {
        List<string> mistakes = GetGeneralMistakes();
        string aux = "";
        foreach(string mistake in mistakes) {
            aux += mistake + "\n";
        }

        StartCoroutine(TypewriterEffect(aux, individualMistakes));
    }

    public void ShowGeneralHits() {
        List<string> hits = GetGeneralHits();
        string aux = "";
        foreach(string hit in hits) {
            aux += hit + "\n";
        }

        StartCoroutine(TypewriterEffect(aux, individualHits));
    }

    private void End() {
        canContinue = false;
        mistakesAndHitsCanvas.SetActive(false);
        feedbackCanvas.SetActive(false);
        endCanvas.SetActive(true);
        PlayerPrefs.DeleteAll();
    }

    public void EndGame() {
        if(formsToggle.isOn)
            Process.Start(SaveSystem.formsUrl);
        Application.Quit();
    }

    void ShowFeedback() {
        feedbackText.text = "";
        canContinue = false;
        continueButton.SetActive(canContinue);
        string aux = "";
        int index = 0;
        int lenght = feedback.Count();

        foreach(string item in feedback) {
            if(index + 1 != lenght)
                aux += item + "\n\n";
            else
                aux += item;
            index++;
        }

        StartCoroutine(TypewriterEffect(aux, feedbackText));

        using(StreamWriter streamWriter = new StreamWriter(individualFeedbackFilePath)) {
            streamWriter.WriteLine(aux);
        }
    }

    IEnumerator TypewriterEffect(string text, Text uiText) {
        foreach(char character in text) {
            uiText.text += character;
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(2f);

        canContinue = true;
        continueButton.SetActive(canContinue);
        continueButton2.SetActive(canContinue);
        count++;
    }

    IEnumerator TypewriterEffect2(string text, Text uiText) {
        foreach(char character in text) {
            uiText.text += character;
            yield return new WaitForSeconds(0.02f);
        }
    }



    private void Increment(int option) {
        switch(option) {
            case 0:
                teamMeetingMistakes++;
                break;

            case 1:
                clientMeetingMistakes++;
                break;

            case 2:
                developmentMistakes++;
                break;

            case 3:
                teamMeetingHits++;
                break;

            case 4:
                clientMeetingHits++;
                break;

            case 5:
                developmentHits++;
                break;

            case 6:
                timeouts++;
                break;
        }
    }
    public List<int> GetIndividualStats() {
        timeouts = 0;
        teamMeetingMistakes = 0;
        clientMeetingMistakes = 0;
        developmentMistakes = 0;
        teamMeetingHits = 0;
        clientMeetingHits = 0;
        developmentHits = 0;

        string[] files = Directory.GetFiles(path);
        int length = files.Length;

        for(int i = 0; i < length; i++) {
            Decision data = SaveSystem.Load(i);

            if(data != null) {
                if(data.isMistake) {
                    if(data.scenery.Equals("Reuniao Equipe")) {
                        Increment(0);
                    }
                    else if(data.scenery.Equals("Reuniao Cliente")) {
                        Increment(1);
                    }
                    else if(data.scenery.Equals("Desenvolvimento")) {
                        Increment(2);
                    }
                    else {
                        Increment(6);
                    }
                }
                else {
                    if(data.scenery.Equals("Reuniao Equipe")) {
                        Increment(3);
                    }
                    else if(data.scenery.Equals("Reuniao Cliente")) {
                        Increment(4);
                    }
                    else {
                        Increment(5);
                    }
                }
            }
        }

        List<int> mistakes = new List<int>() {
            timeouts,              // 0
            teamMeetingMistakes,   // 1
            clientMeetingMistakes, // 2
            developmentMistakes,   // 3
            teamMeetingHits,       // 4
            clientMeetingHits,     // 5
            developmentHits        // 6
        };

        return mistakes;
    }

    public int GetAllHits() {
        return (teamMeetingHits + clientMeetingHits + developmentHits);
    }

    public int GetAllMistakes() {
        return (timeouts + teamMeetingMistakes + clientMeetingMistakes + developmentMistakes);
    }

    public List<string> GetMistakes() {
        string[] files = Directory.GetFiles(path);
        int length = files.Length;
        List<string> mistakes = new List<string>();

        //Debug.Log(length);

        for(int i = 0; i < length; i++) {
            Decision data = SaveSystem.Load(i);
            if(data != null)
                if(data.isMistake)
                    mistakes.Add(data.decisionId);
        }

        mistakes = mistakes.Distinct().ToList();

        return mistakes;
    }

    public List<string> GetHits() {
        string[] files = Directory.GetFiles(path);
        int length = files.Length;
        List<string> hits = new List<string>();

        //Debug.Log(length);

        for(int i = 0; i < length; i++) {
            Decision data = SaveSystem.Load(i);
            if(data != null)
                if(!data.isMistake)
                    hits.Add(data.decisionId);
        }

        hits = hits.Distinct().ToList();

        return hits;
    }

    public List<int> GetGeneralStats() {
        GeneralInfo groupData = SaveSystem.LoadGeneralInfo();
        return new List<int>() {
            groupData.timeouts,
            groupData.teamMeetingMistakes,
            groupData.clientMeetingMistakes,
            groupData.developmentMistakes,
            groupData.teamMeetingHits,
            groupData.clientMeetingHits,
            groupData.developmentHits
        };
    }

    public List<string> GetGeneralMistakes() {
        GeneralInfo groupData = SaveSystem.LoadGeneralInfo();
        List<string> mistakes = new List<string>();

        //Debug.Log(groupData.timeouts);

        if(groupData.timeouts != 0)
            mistakes.Add("O grupo demorou para realizar as ações.\n");

        if(groupData.clientMeetingMistakes != 0)
            mistakes.Add("Erros na comunicação com o cliente.\n");

        if(groupData.teamMeetingMistakes != 0)
            mistakes.Add("Erros na comunicação com a equipe.\n");

        if(groupData.developmentMistakes != 0)
            mistakes.Add("Erros no desenvolvimento.\n");

        return mistakes;
    }

    public List<string> GetGeneralHits() {
        GeneralInfo groupData = SaveSystem.LoadGeneralInfo();
        List<string> hits = new List<string>();

        if(groupData.clientMeetingHits != 0)
            hits.Add("Sua comunicação com o cliente foi boa.\n");

        if(groupData.teamMeetingHits != 0)
            hits.Add("Sua comunicação com a equipe foi boa.\n");

        if(groupData.developmentHits != 0)
            hits.Add("O desenvolvimento foi adequado.\n");

        return hits;
    }
}
