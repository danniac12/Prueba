using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    // public List<Datos.Db> dbs;
    public List <GameObject> estudiantesCard;
    public List <GameObject> estudiantesCard2;

    public GameObject estudiantePrefab;
    public GameObject estudiantePrefab2;
    public GameObject partentPantalla1;
    public GameObject pantall2;
    public GameObject partentPantalla2;

    public GameObject aprovadoPanel;
    public GameObject reprovadoPanel;
    public TMP_InputField jsonText;
    public GameObject alerta;
    public Datos datos;

    public RectTransform parentCard;
    public RectTransform aprobado;
    public RectTransform reprobado;

    
    // Start is called before the first frame update
    void Start()
    {
        LoadJson();
      

    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            partentPantalla2.GetComponent<VerticalLayoutGroup>().enabled = false;
            aprovadoPanel.GetComponent<LayoutGroup>().enabled = false;  
                reprovadoPanel.GetComponent<LayoutGroup>().enabled = false;
        }
        else
        {
            partentPantalla2.GetComponent<VerticalLayoutGroup>().enabled = true;
            aprovadoPanel.GetComponent<LayoutGroup>().enabled = true;
                reprovadoPanel.GetComponent<LayoutGroup>().enabled = true;
        }
    }
    void LoadJson()
    {
        string json = File.ReadAllText(Application.streamingAssetsPath + "/estudiantes.json");
        Debug.Log(json);
        jsonText.text = json;
        
        datos = JsonUtility.FromJson<Datos>(json);
       

        for (int i = 0; i < datos.data.Count; i++)
        {
            if(partentPantalla1.transform.childCount < datos.data.Count)
            {
                GameObject temp = Instantiate(estudiantePrefab, partentPantalla1.transform);
                estudiantesCard.Add(temp);
            }
            
            if(partentPantalla2.transform.childCount < datos.data.Count && estudiantesCard2.Count < datos.data.Count)
            {
                GameObject temp = Instantiate(estudiantePrefab2, partentPantalla2.transform);
                temp.transform.GetChild(2).GetComponent<DragWindow>().ParentRectTransfom = parentCard;
                temp.transform.GetChild(2).GetComponent<DragWindow>().AprobadoRectTransfom = aprobado;
                temp.transform.GetChild(2).GetComponent<DragWindow>().ReprobadoRectTransfom = reprobado;
                temp.transform.GetChild(2).GetComponent<DragWindow>().note = datos.data[i].note;
                temp.transform.GetChild(2).GetComponent<DragWindow>().nameStudent = datos.data[i].name;

                estudiantesCard2.Add(temp);
            }

        }
        for (int i = 0; i < estudiantesCard.Count; i++)
        {
            if(i < datos.data.Count)
            {
                estudiantesCard[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Nombre: " + datos.data[i].name + "\n" + "Apellido: " + datos.data[i].lastName + "\n" + "Correo: " + datos.data[i].email + "\n" + "Codigo: " + datos.data[i].id + "\n" + "Nota: " + datos.data[i].note;

            }
            else
            {

                Destroy(estudiantesCard[i]);
                estudiantesCard.RemoveAt(estudiantesCard.Count - 1);

            }
          
        }
        
        for (int i = 0; i < estudiantesCard2.Count; i++)
        {
            if (i < datos.data.Count)
            {
                estudiantesCard2[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Nombre: " + datos.data[i].name + "\n" + "Apellido: " + datos.data[i].lastName + "\n" + "Correo: " + datos.data[i].email + "\n" + "Codigo: " + datos.data[i].id + "\n" + "Nota: " + datos.data[i].note;

            }
            else
            {

                Destroy(estudiantesCard2[i]);
                estudiantesCard2.RemoveAt(estudiantesCard2.Count - 1);

            }

        }

       // partentPantalla2.GetComponent<VerticalLayoutGroup>().enabled = false;
    }
    
    public void Verify()
    {
        bool apruv = false;
        bool allStudentsCheck = false;
        for (int i = 0; i < datos.data.Count; i++)
        {

            if(datos.data[i].note >= 3.0f)
            {
                apruv = true;
            }
            else
            {
                apruv = false;
            }
            for (int j = 0; j < estudiantesCard.Count; j++)
            {
                if (estudiantesCard[j].transform.GetChild(1).GetComponent<Toggle>().isOn || estudiantesCard[j].transform.GetChild(2).GetComponent<Toggle>().isOn)
                {

                    if (estudiantesCard[j].transform.GetChild(1).GetComponent<Toggle>().isOn && estudiantesCard[j].transform.GetChild(2).GetComponent<Toggle>().isOn)
                    {
                        allStudentsCheck = false;
                        apruv = false;
                        alerta.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "el juicio del estudiante <color=red>" + datos.data[j].name + "</color> no es claro";
                        alerta.SetActive(true);
                        break;
                    }
                    else
                    {
                        allStudentsCheck = true;
                    }
                }
                else
                {
                    allStudentsCheck = false;
                    apruv = false;
                    alerta.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "el estudiante <color=red>" + datos.data[j].name + "</color> no ha sido marcado";
                    alerta.SetActive(true);
                    break;
                }
            }

            if (allStudentsCheck)
            {
                if (estudiantesCard[i].transform.GetChild(1).GetComponent<Toggle>().isOn != apruv)
                {
                    alerta.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "el valor de la nota del estudiante <color=red>" + datos.data[i].name + "</color> no corresponde a juicio que a emitido";
                    alerta.transform.GetChild(1).GetComponent<Button>().onClick.RemoveListener(MyAction);
                    alerta.SetActive(true);
                    break;
                }
                else
                {

                    alerta.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "todas las casillas han sido marcadas correctamente";
                    alerta.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(MyAction);

                    alerta.SetActive(true);
                    Debug.Log("Agrega Accion");

                }
            }


        }

       
    }

    public void Verificar()
    {
        bool allRight= false;
        int studentsTotal = aprovadoPanel.transform.childCount + reprovadoPanel.transform.childCount; 
        if(studentsTotal == estudiantesCard2.Count)
        {
            Debug.Log("todos los estudiantes han sido asignados");
            if(reprovadoPanel.transform.childCount > 0)
            {
                for (int i = 0; i < reprovadoPanel.transform.childCount; i++)
                {
                    if(reprovadoPanel.transform.GetChild(i).GetChild(2).GetComponent<DragWindow>().note >= 3)
                    {
                        Debug.Log("todos los estudiantes han sido asignados");
                        alerta.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "el valor de la nota del estudiante <color=red>" + reprovadoPanel.transform.GetChild(i).GetChild(2).GetComponent<DragWindow>().nameStudent + "</color> no corresponde a juicio que a emitido";
                        alerta.SetActive(true);
                        allRight = false;
                        break;
                    }
                    else
                    {
                        allRight = true;
                    }
                    for (int j = 0; j < aprovadoPanel.transform.childCount; j++)
                    {
                        if (aprovadoPanel.transform.GetChild(j).GetChild(2).GetComponent<DragWindow>().note < 3)
                        {
                            Debug.Log("todos los estudiantes han sido asignados");
                            alerta.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "el valor de la nota del estudiante <color=red>" + aprovadoPanel.transform.GetChild(j).GetChild(2).GetComponent<DragWindow>().nameStudent + "</color> no corresponde a juicio que a emitido";
                            alerta.SetActive(true);
                            allRight = false;
                            break;
                        }
                        else
                        {
                            allRight = true;
                        }
                    }
                }


            }
            else
            {
                for (int j = 0; j < aprovadoPanel.transform.childCount; j++)
                {
                    if (aprovadoPanel.transform.GetChild(j).GetChild(2).GetComponent<DragWindow>().note < 3)
                    {
                        Debug.Log(j);
                        alerta.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "el valor de la nota del estudiante <color=red>" + aprovadoPanel.transform.GetChild(j).GetChild(2).GetComponent<DragWindow>().nameStudent + "</color> no corresponde a juicio que a emitido";
                        alerta.SetActive(true);
                        allRight = false;
                        break;
                    }
                    else
                    {
                        allRight = true;
                    }
                    for (int i = 0; i < reprovadoPanel.transform.childCount; i++)
                    {
                        if (reprovadoPanel.transform.GetChild(i).GetChild(2).GetComponent<DragWindow>().note >= 3)
                        {
                            Debug.Log("todos los estudiantes han sido asignados");
                            alerta.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "el valor de la nota del estudiante <color=red>" + reprovadoPanel.transform.GetChild(i).GetChild(2).GetComponent<DragWindow>().nameStudent + "</color> no corresponde a juicio que a emitido";
                            alerta.SetActive(true);
                            allRight = false;
                            break;
                        }
                        else
                        {
                            allRight = true;
                        }

                    }
                }
              

            }
            if (allRight)
            {
                alerta.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Todos los estudiantes han sido asignados correctamente";
                alerta.SetActive(true);
            }
        }
        else
        {
            alerta.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text ="falta estudiantes por asignar";
        }


    }

    public void SaveJson(TMP_InputField file)
    {
        File.WriteAllText(Application.streamingAssetsPath + "/estudiantes.json", file.text);
        LoadJson();
    }

    public void MyAction()
    {
        pantall2.SetActive(true);
        alerta.SetActive(false);
    }
   
    public void Salir()
    {
        Application.Quit();
    }
}
    [Serializable]
public class Datos
{

    [Serializable]
    public struct Db
    {
            public string name;
            public string lastName;
            public string email;
            public string id;
            public float note;
    }
    public List<Db> data;


}