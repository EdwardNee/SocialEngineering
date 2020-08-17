using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public QuestionList[] questions;
    public Text[] answersText;    //Ответы
    public Text QText;
    public GameObject headPanel;
    public Button[] answerBttns = new Button[3];

    public Sprite[] TFIcons = new Sprite[2];
    public Image TFIcon;
    public Text TFText;

    private List<object> qList;
    private QuestionList crntQ;
    private int randQ;

    private int rightAnsw;
    public Text rightAnswText;

    public Text thatIsGood;
    public string[] thatisGood;

    public Image imgForQuestion;    //Картинка для вопроса(Если такая есть)
    public Sprite[] imgSprites; //Картинки для вопросов
    private List<Sprite> imgSList;

    public void OnClickPlay()
    {
        rightAnsw = 0;
        imgSList = new List<Sprite>(imgSprites);
        qList = new List<object>(questions);    //Листу присвоятся все вопросы и ответы из массива questions
        questionGenerate();
        if (!headPanel.GetComponent<Animator>().enabled) headPanel.GetComponent<Animator>().enabled = true; //Включение анимации
        else headPanel.GetComponent<Animator>().SetTrigger("In");

        //rightL[0] = "DGDFDFDF";
        //textOfRightAns.text = (string)rightL[0];
    }

    void questionGenerate()
    {
        if (qList.Count > 0)
        {
            print(imgSList.Count);

            
            randQ = Random.Range(0, qList.Count);
            crntQ = qList[randQ] as QuestionList;
            QText.text = crntQ.question;
            print("OK");
            if (imgSList[randQ] != null)
            {
                imgForQuestion.gameObject.SetActive(true);
                print("Hello Its Me");
                
                //imgForQuestion.gameObject.GetComponent<Animator>().SetTrigger("In");
                imgForQuestion.sprite = imgSList[randQ];
            }
            List<string> answers = new List<string>(crntQ.answers);  //Сюда переносятся все ответы на вопросы.
            for (int i = 0; i < crntQ.answers.Length; i++)
            {
                int rand = Random.Range(0, answers.Count);
                answersText[i].text = answers[rand];    //Присваивание рандомного вар ответа.
                answers.RemoveAt(rand);
            }
            StartCoroutine(animBttns());
        }
        else
        {
            print("Вы прошли игру");
            int j = 0;
            while (j < answerBttns.Length)
            {
                if (answerBttns[j].gameObject.activeSelf) answerBttns[j].gameObject.SetActive(false);
                j++;
            }
            headPanel.GetComponent<Animator>().SetTrigger("Out");
            rightAnswText.text = $"Правильных ответов:{rightAnsw} из {questions.Length}";
        }

        IEnumerator animBttns()
        {
            for (int i = 0; i < answerBttns.Length; i++)
            {
                answerBttns[i].interactable = false;    //Делаем кнопки массива некликабельными
                yield return new WaitForSeconds(0.2f);
            }
            int j = 0;

            while (j < answerBttns.Length)
            {
                if (!answerBttns[j].gameObject.activeSelf) answerBttns[j].gameObject.SetActive(true);
                else answerBttns[j].gameObject.GetComponent<Animator>().SetTrigger("In");
                j++;
                yield return new WaitForSeconds(0.3f);    //Ожидание, чтобы проигралась анимация и загенерировался вопрос
            }
            //
            //
            //Тут лучще сделать паузы
            for (int i = 0; i < answerBttns.Length; i++)
            {
                answerBttns[i].interactable = true;    //Делаем кнопки массива кликабельными
            }
            yield break;
        }
    }

    /// <summary>
    /// Логика при том или ином данном ответе.
    /// </summary>
    /// <param name="check">Переменная верности ответа.</param>
    /// <returns></returns>
    IEnumerator trueOrFalse(bool check)
    {

        if (imgSList[randQ] != null)
        {
            imgForQuestion.gameObject.SetActive(false);
            //imgForQuestion.gameObject.GetComponent<Animator>().SetTrigger("Out");
            //imgForQuestion.gameObject.GetComponent<Animator>().SetTrigger("OutTo");
        }
        for (int i = 0; i < answerBttns.Length; i++)
        {
            answerBttns[i].interactable = false;    //Делаем кнопки массива некликабельными
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < answerBttns.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            answerBttns[i].gameObject.SetActive(false);
        }

        //if (!TFIcon.gameObject.activeSelf)
        //{
        //    TFIcon.gameObject.SetActive(true);
        //    TFText.gameObject.SetActive(true);
        //}
        //Включение анимации кнопки, если кнопки уже активны
        //else
        //{

        TFIcon.gameObject.GetComponent<Animator>().SetTrigger("In");


        //}
        //for (int i = 0; i < answerBttns.Length; i++) answerBttns[i].gameObject.GetComponent<Animator>().SetTrigger("Out");
        //QText.gameObject.GetComponent<Animator>().SetTrigger("Out");

        if (check)
        {
            rightAnsw++;
            //Тут должно быть убирание возможности нажимать на кноки
            TFIcon.sprite = TFIcons[0];
            TFText.text = "Правильный ответ";
            TFText.color = Color.green;
            yield return new WaitForSeconds(1.8f);

            //thatIsGood.text = thatisGood[randQ];    //Показываем правильный ответ-пояснение
            TFIcon.gameObject.GetComponent<Animator>().SetTrigger("Out");
            TFIcon.gameObject.GetComponent<Animator>().SetTrigger("OutTo");

            //if (imgSList[randQ] != null)
            //{
            //    imgForQuestion.gameObject.SetActive(false);
            //    //imgForQuestion.gameObject.GetComponent<Animator>().SetTrigger("Out");
            //    //imgForQuestion.gameObject.GetComponent<Animator>().SetTrigger("OutTo");
            //}
            //imgForQuestion.gameObject.GetComponent<Animator>().SetTrigger("In");
            qList.RemoveAt(randQ);
            yield return new WaitForSeconds(1f);    //Ожидание, чтобы проигралась анимация и загенерировался вопрос
            questionGenerate();
            yield break;
        }
        else
        {
            TFIcon.sprite = TFIcons[1];
            TFText.text = "Неправильный ответ";
            TFText.color = Color.red;
            yield return new WaitForSeconds(1.8f);
            //TFIcon.gameObject.SetActive(false);
            //TFText.gameObject.SetActive(false);

            //thatIsGood.text = thatisGood[randQ];    //Показываем правильный ответ-пояснение
            //yield return new WaitForSeconds(0.5f);
            TFIcon.gameObject.GetComponent<Animator>().SetTrigger("Out");
            TFIcon.gameObject.GetComponent<Animator>().SetTrigger("OutTo");

            if (imgSList[randQ] != null)
            {
                imgForQuestion.gameObject.SetActive(false);
                //imgForQuestion.gameObject.GetComponent<Animator>().SetTrigger("Out");
                //imgForQuestion.gameObject.GetComponent<Animator>().SetTrigger("OutTo");
            }
            imgSList.RemoveAt(randQ);
            qList.RemoveAt(randQ);
            yield return new WaitForSeconds(1f);    //Ожидание, чтобы проигралась анимация и загенерировался вопрос
            questionGenerate();
            yield break;
        }
    }

    /// <summary>
    /// Метод, проверяющий выбор игрока: Правильный или неправильный ответ.
    /// </summary>
    /// <param name="index">индекс нажатой кнопки.</param>
    public void AnswersBttns(int index)
    {
        if (answersText[index].text.ToString() == crntQ.answers[0])
        {
            StartCoroutine(trueOrFalse(true));
        }
        else
        {
            StartCoroutine(trueOrFalse(false));
        }
    }
}

[System.Serializable] //Этот атрибут говорит системе, что данный класс надо пытаться сериализовать.
//Без этого объект этого класса не будет сохранять своё состояние при входе/выходе из игры,
//а также для него не будет работать редактирование через инспектор.(http://www.unity3d.ru/distribution/viewtopic.php?f=105&t=38553)
public class QuestionList //Класс вопросов    
{
    public string question;
    public string[] answers = new string[3];
}