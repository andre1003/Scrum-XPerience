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
    public GameObject mapCanvas;
    public GameObject pauseMenuCanvas;

    public GameObject postProcessing;

    public Text explanationText;
    public List<GameObject> phases;

    private int currentPhase;

    private void Awake() {
        if(PlayerPrefs.GetInt("post_processing") == 1) {
            postProcessing.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start() {
        LockOrUnlockPlayer();
        currentPhase = 0;
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.M)) {
            mapCanvas.SetActive(!mapCanvas.activeSelf);
        }
        else if(Input.GetKeyDown(KeyCode.Escape)) {
            if(pauseMenuCanvas.activeSelf == false) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            LockOrUnlockPlayer();
            pauseMenuCanvas.SetActive(!pauseMenuCanvas.activeSelf);
        }

        if(PlayerPrefs.GetInt("post_processing") == 1) {
            postProcessing.SetActive(true);
        }
        else {
            postProcessing.SetActive(false);
        }

        if(explanationCanvas.activeSelf) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            movementController.enabled = false;
            mouseController.enabled = false;
            animator.SetBool("isMoving", false);
        }
    }

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
        if(phase < 6) {
            Destroy(phases[phase]);
            if(phase != 6) {
                phases[phase + 1].SetActive(true);
            }
        }

        switch(phase) {
            case 0:
                explanationText.text = "Muito bem! Agora vamos come�ar a conhecer os cen�rios.\n\nPrimeiro, vamos para a Sala de Desenvolvimento, que j� est� aqui perto. Olhe ao redor e se aproxime do marcador.";
                break;

            case 1:
                explanationText.text = "Certo, agora vamos para a Sala de Reuni�o com o Cliente.\n\nEla fica ao lado da Sala de Desenvolvimento (onde voc� est� agora) e � t�o grande quanto.\n\nPara facilitar encontr�-la, ao chegar na entrada da sala, voc� poder� ver uma TV na parede.";
                break;

            case 2:
                explanationText.text = "OK, estamos indo bem! Agora vamos para a Sala de Reuni�o com a Equipe.\n\nPara chegar at� l�, volte para a Sala de Desenvolvimento e no final dela voc� ver� duas entradas, uma ao lado da outra. A Sala de Reuni�o com a Equipe � a entrada da ESQUERDA.";
                break;

            case 3:
                explanationText.text = "Agora vamos para a �ltima sala, a Sala de Reuni�o com a Diretoria.\n\nLembra de que para chegar na Sala de Reuni�o com a Equipe voc� entrou na entrada da esquerda? Agora voc� vai entrar na entrada da DIREITA.\n\nVamos l�!";
                break;

            case 4:
                explanationText.text = "Muito bem! Agora voc� conhece todo o mapa do jogo.\n\nVamos aprender como funciona o jogo! Para isso, v� at� a Sala de Desenvolvimento novamente.";
                break;

            case 5:
                explanationText.text = "Durante a partida, voc� ir� selecionar o papel que deseja desempenhar, de acordo com a metodologia escolhida.\n\nDesse modo, ao se aproximar de cada sala, com exce��o da Sala de Reuni�o com a Diretoria, uma tela ser� disponibilizada para que voc� fa�a sua escolha.\n\nAo fechar essa janela, uma tela de exemplo ser� apresentada para voc�.";
                break;

            case 6:
                explanationText.text = "Muito bem! Agora voc� sabe como voc� vai fazer as escolhas durante a partida.\n\nAlguns papeis podem visualizar o progresso da partida, sendo eles o Scrum Master, o Product Owner e o Gerente de Projetos. As pessoas encarregadas por esses cargos devem comunicar o restante da equipe para conduzir a partida da melhor forma poss�vel. Vamos ver melhor isso na Sala de Reuni�o com a Diretoria.";
                break;

            case 7:
                explanationText.text = "Aqui voc� poder� ver como est� o andamento da partida, verificando se o Cliente e a Equipe est�o Felizes, Neutros ou Infelizes.\n\nEsse status varia conforme o n�mero de erros e acertos do grupo. Al�m disso, � poss�vel verificar o progresso geral da partida.\n\nA tela a seguir representa o Progresso do jogo.";
                break;

            case 8:
                explanationText.text = "Excelente! Agora voc� sabe como o jogo funciona!\n\nPara finalizarmos, ser� disponibilizado a tela de HUD, em que na esquerda ser� apresentado o n�mero de erros e acertos, no meio ser� apresentado o tempo restante para a rodada e na direita ser� apresentada a rodada e o turno da partida.";
                break;

            case 9:
                explanationText.text = "Muito bem, estamos quase terminando. S� me deixe explicar como ganhar o jogo.\n\nO objetivo da partida � realizar o maior n�mero de acertos poss�veis, mantendo tanto o Cliente quanto a Equipe felizes. A partida termina quando XX erros forem cometidos ou quando as 20 rodadas forem finalizadas. Se o tempo se esgotar e ainda restarem escolhas a serem feitas, elas ser�o consideradas como erros.";
                break;

            case 10:
                explanationText.text = "E por hoje � s�! Agora voc� pode enfrentar uma partida de verdade com seus amigos.\n\nPara isso, junte uma equipe de 3 jogadores e inicie uma partida.\n\nAh, j� ia esquecendo, quando voc� finalizar a partida, seus acertos e erros resultar�o em uma pontua��o cumulativa, a qual gera um ranking dos grupos, disponibilizado no site do jogo! Por isso, se dedique ao m�ximo para ser o top 1 dos grupos!";
                break;

            default:
                Debug.Log("Fase 1 conclu�da");
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
        PhaseSwitch(6);
    }
}
