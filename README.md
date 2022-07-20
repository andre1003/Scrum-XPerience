# Scrum XPerience

<!---Esses são exemplos. Veja https://shields.io para outras pessoas ou para personalizar este conjunto de escudos. Você pode querer incluir dependências, status do projeto e informações de licença aqui--->

![GitHub repo size](https://img.shields.io/github/repo-size/andre1003/Scrum-XPerience?style=for-the-badge)
![GitHub language count](https://img.shields.io/github/languages/count/andre1003/Scrum-XPerience?style=for-the-badge)
![GitHub forks](https://img.shields.io/github/forks/andre1003/Scrum-XPerience?style=for-the-badge)
![Bitbucket open issues](https://img.shields.io/bitbucket/issues/andre1003/Scrum-XPerience?style=for-the-badge)
![Bitbucket open pull requests](https://img.shields.io/bitbucket/pr-raw/andre1003/Scrum-XPerience?style=for-the-badge)

<img src="scrum-xperience.png" alt="Jogo em execução">

> Jogo educativo voltado para o ensino das metodologias ágeis Scrum e eXtreme Programming. Para isso, foi utilizado Unity, juntamente com a linguagem C#.

### Ajustes e melhorias

O projeto ainda está em desenvolvimento e as próximas atualizações serão voltadas nas seguintes tarefas:

* Correções no multiplayer
* Níveis de dificuldade
* Escolhas em grupo
* Adição de cenários e metodologias
* Adição de recursos sonoros
* Novos idiomas

## 💻 Pré-requisitos

Antes de começar, verifique se você atendeu aos seguintes requisitos:
<!---Estes são apenas requisitos de exemplo. Adicionar, duplicar ou remover conforme necessário--->
* Você possui o software Unity instalado na versão `2020.3.0.0f1 ou superior`
* Você tem uma máquina `Windows (10 ou superior) / Linux / Mac`. Obs.: Para melhor performance, utilize Windows

## 🚀 Instalando o Scrum XPerience

Para instalar o Scrum XPerience, basta baixar o código e adicioná-lo ao Unity HUB. Com a versão correta do software, você poderá abrir o projeto sem problemas. No entanto, antes de utilizar o Scrum XPerience é necessário realizar algumas configurações de conexão com o site. Para isso, você deve editar o arquivo ```Scripts/SaveController/SaveSystem.cs``` da seguinte forma:

```cs
public static string groupRegisterUrl = "URL de cadastro de grupo";
public static string loginUrl = "URL de login do jogo";
public static string matchRegisterUrl = "URL de cadastro de partida";
public static string decisionRegisterUrl = "URL de cadastro de decisão";
public static string homeUrl = "URL da página principal do site";
public static string formsUrl = "URL do formulário avaliativo";
```

O código do site do Scrum XPerience, assim como o passo a passo de como configurá-lo pode ser acessado [aqui](https://github.com/andre1003/GameWebsite).

Após isso, o Scrum XPerience pode ser utilizado, conforme os passos descritos na seção a seguir.

## ☕ Usando o Scrum XPerience

Para usar Scrum XPerience, siga estas etapas:

1. Abra o software com o Unity
2. Acesse a aba `File` e clique em `Build and Run`

Pronto! Você já está rodando a aplicação. Você também pode obtê-la [aqui](https://www.dcce.ibilce.unesp.br/sxp).

[⬆ Voltar ao topo](#scrum-xperience)<br>
