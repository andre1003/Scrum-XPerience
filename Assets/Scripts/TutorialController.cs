using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour {
    public MovementController movementController;
    public MouseController mouseController;
    public Animator animator;

    public GameObject explanationCanvas;
    public GameObject choiceCanvas;
    public GameObject statsCanvas;
    public GameObject hudCanvas;

    public Text explanationText;
    public List<GameObject> phases;

    private int currentPhase;

    // Start is called before the first frame update
    void Start() {
        //LockAndHideCursor();
        LockOrUnlockPlayer();
        currentPhase = 0;
    }

    // Update is called once per frame
    void Update() {

    }

    //public void SetMovementController(MovementController movementController) {
    //    this.movementController = movementController;
    //}

    //public void SetMouseController(MouseController mouseController) {
    //    this.mouseController = mouseController;
    //}

    public void LockOrUnlockPlayer() {
        movementController.enabled = !movementController.enabled;
        mouseController.enabled = !mouseController.enabled;
        animator.SetBool("isMoving", mouseController.enabled);
    }

    public void LockAndHideCursor() {
        if(Cursor.visible) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    public void PhaseSwitch(int phase) {
        currentPhase = phase;
        if(phase < 7) {
            Destroy(phases[phase]);
            if(phase != 7) {
                phases[phase + 1].SetActive(true);
            }
        }

        LockOrUnlockPlayer();
        LockAndHideCursor();

        switch(phase) {
            case 0:
                explanationText.text = "Muito bem! Agora vamos começar a conhecer os cenários.\n\nPrimeiro, vamos para a Sala de Desenvolvimento, que já está aqui perto. Olhe ao redor e se aproxime do marcador.";
                break;

            case 1:
                explanationText.text = "Certo, agora vamos para a Sala de Reunião com o Cliente.\n\nEla fica ao lado da Sala de Desenvolvimento (onde você está agora) e é tão grande quanto.\n\nPara facilitar encontrá-la, ao chegar na entrada da sala, você poderá ver uma TV na parede.";
                break;

            case 2:
                explanationText.text = "OK, estamos indo bem!Agora vamos para a Sala de Reunião com a Equipe\n\nPara chegar até lá, volte para a Sala de Desenvolvimento e no final dela você verá duas entradas, uma ao lado da outra. A Sala de Reunião com a Equipe é a entrada da ESQUERDA.";
                break;

            case 3:
                explanationText.text = "Agora vamos para a última sala, a Sala de Reunião com a Diretoria.\n\nLembra de que para chegar na Sala de Reunião com a Equipe você entrou na entrada da esquerda? Agora você vai entrar na entrada da DIREITA.\n\nVamos lá!";
                break;

            case 4:
                explanationText.text = "Muito bem! Agora você conhece todo o mapa do jogo.\n\nVamos aprender como funciona o jogo! Para isso, vá até a Sala de Desenvolvimento novamente.";
                break;

            case 5:
                explanationText.text = "Durante a partida, você irá selecionar o papel que deseja desempenhar, de acordo com a metodologia escolhida.\n\nDesse modo, ao se aproximar de cada sala, com exceção da Sala de Reunião com a Diretoria, uma tela será disponibilizada para que você faça sua escolha.\n\nAo fechar essa janela, uma tela de exemplo será apresentada para você.";
                break;

            case 6:
                explanationText.text = "Muito bem! Agora você sabe como você vai fazer as escolhas durante a partida.\n\nAlguns papeis podem visualizar o progesso da partida, sendo eles o Scrum Master, o Product Owner e o Gerente de Projetos. As pessoas encarregadas por esses cargos devem comunicar o restante da equipe para conduzir a partida da melhor forma possível. Vamos ver melhor isso na Sala de Reunião com a Diretoria.";
                break;

            case 7:
                explanationText.text = "Aqui você poderá ver como está o andamento da partida, verificando se o Cliente e a Equipe estão Felizes, Neutros ou Infelizes.\n\nEsse status varia conforme o número de erros e acertos do grupo. Além disso, é possível verificar o progresso geral da partida.\n\nA tela a seguir representa o Progresso do jogo.";
                break;

            case 8:
                LockOrUnlockPlayer();
                LockAndHideCursor();
                explanationText.text = "Excelente! Agora você sabe como o jogo funciona!\n\nPara finalizarmos, será disponibilizado a tela de HUD, em que na esquerda será apresentado o número de erros e acertos, no meio será apresentado o tempo restante para a rodada e na direita será apresentada a rodada e o turno da partida.";
                break;

            case 9:
                LockOrUnlockPlayer();
                LockAndHideCursor();
                explanationText.text = "Muito bem, estamos quase terminando. Só me deixe explicar como ganhar o jogo.\n\nO objetivo da partida é realizar o maior número de acertos possíveis, mantendo tanto o Cliente quanto a Equipe felizes. A partida termina quando XX erros forem cometidos ou quando as 20 rodadas forem finalizadas. Se o tempo se esgotar e ainda restarem escolhas a serem feitas, elas serão consideradas como erros.";
                break;

            case 10:
                LockOrUnlockPlayer();
                LockAndHideCursor();
                explanationText.text = "E por hoje é só! Agora você pode enfrentar uma partida de verdade com seus amigos.\n\nPara isso, junte uma equipe de 3 jogadores e inicie uma partida.\n\nAh, já ia esquecendo, quando você finalizar a partida, seus acertos e erros resultarão em uma pontuação cumulativa, a qual gera um ranking dos grupos, disponibilizado no site do jogo! Por isso, se dedique ao máximo para ser o top 1 dos grupos!";
                break;

            default:
                Debug.Log("Fase 1 concluída");
                break;
        }

        explanationCanvas.SetActive(true);
    }

    public void CloseExplanation() {
        if(currentPhase == 5) {
            choiceCanvas.SetActive(true);
            currentPhase++;
        }
        else if(currentPhase == 7) {
            statsCanvas.SetActive(true);
            currentPhase++;
        }
        else if(currentPhase == 8) {
            hudCanvas.SetActive(true);
            currentPhase++;
        }
        else if(currentPhase == 9) {
            PhaseSwitch(9);
            currentPhase++;
        }
        else if(currentPhase == 10) {
            PhaseSwitch(10);
            currentPhase++;
        }
        else if(currentPhase == 11) {
            SceneManager.LoadScene(0);
        }
        else {
            LockOrUnlockPlayer();
            LockAndHideCursor();
        }
    }

    public void OpenPhaseSix() {
        currentPhase = 6;
        phases[currentPhase].SetActive(true);
    }
}
