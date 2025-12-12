using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ChangeMenu : MonoBehaviour
{
    [SerializeField] GameObject blackSquare;
    [SerializeField] Canvas canvas;

    public void OnEnable()
    {
        PlayerSpaceShips.points = 0;
        PlayerSpaceShips.gameEnded = false;
        StartCoroutine(AnimacionEntrada());
    }

    public void Transicion(string nombre)
    {
        StartCoroutine(AnimacionSalida(nombre));
    }

    IEnumerator AnimacionSalida(string nombre)
    {
        List<GameObject> cuadros = new List<GameObject>();
        int filas = 9;
        int columnas = 16;

        blackSquare.SetActive(true);

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                GameObject temporal = Instantiate(blackSquare, Vector3.zero, Quaternion.identity, canvas.transform);
                temporal.GetComponent<RectTransform>().anchoredPosition = new Vector2 (120 * j, 120 * i);
                Color cTemporal = new Color(0f, 0f, 0f, 0f);
                temporal.GetComponent<Image>().color = cTemporal;
                cuadros.Add(temporal);
            }
        }

        blackSquare.SetActive(false);

        int random;

        while (cuadros.Count > 0)
        {
            random = Random.Range(0, cuadros.Count);
            if (cuadros.Count == 1) StartCoroutine(FadeIn(cuadros[random], true, nombre));
            else StartCoroutine(FadeIn(cuadros[random], false, nombre));
            cuadros.RemoveAt(random);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator FadeIn(GameObject a, bool salida, string nombre)
    {
        
        float aValue = 0f;
        while (aValue < 1)
        {
            yield return new WaitForSeconds(0.2f);
            aValue += 0.2f;
            Color temporal = new Color(0f, 0f, 0f, aValue);
            a.GetComponent<Image>().color = temporal;
        }

        if (salida) SceneManager.LoadScene(nombre);
    }


    IEnumerator AnimacionEntrada()
    {
        List<GameObject> cuadros = new List<GameObject>();
        int filas = 9;
        int columnas = 16;
        blackSquare.SetActive(true);

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                GameObject temporal = Instantiate(blackSquare, Vector3.zero, Quaternion.identity, canvas.transform);
                temporal.GetComponent<RectTransform>().anchoredPosition = new Vector2 (120 * j, 120 * i);
                cuadros.Add(temporal);
            }
        }

        blackSquare.SetActive(false);

        int random;

        while (cuadros.Count > 0)
        {
            random = Random.Range(0, cuadros.Count);
            StartCoroutine(FadeOut(cuadros[random]));
            cuadros.RemoveAt(random);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator FadeOut(GameObject a)
    {
        float aValue = 1f;
        while (aValue > 0)
        {
            yield return new WaitForSeconds(0.2f);
            aValue -= 0.2f;
            Color temporal = new Color(0f, 0f, 0f, aValue);
            a.GetComponent<Image>().color = temporal;
        }

        Destroy(a);
    }
}
