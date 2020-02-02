﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class sePuedeRecojer : MonoBehaviour
{
    // Este script ayuda en que si el jugador está en el rango del collider de este objeto
    // le permite presionar espacio y poder recolectar la pieza de la nave espacial

    public enum nombreDePiezas // your custom enumeration
    {
        puerta1,
        pistola4,
        pistola3,
        pistola2,
        pistola1,
        ala1,
        ala2,
        motor4,
        motor3,
        flaps2,
        motor2,
        puerta2,
        motor1,
        flaps1,
        rampa,
        vidrio,
        vidrio2,
        naveatras,
        navedelantera
    };

    [SerializeField]
    private nombreDePiezas nombreDePieza;
    [SerializeField]
    private GameObject jugador;
    [SerializeField]
    [Range(0, 6)]
    private float tiempoParaRecojer;
    [SerializeField]
    private GameObject nave;
    [SerializeField]
    private float minDistanceForHighlight;

    private bool flag = false;

    // AGREGADO X JUAN
    public AudioSource winTheme;
    private AudioSource[] allAudioSources;
    private bool winState = false;

    private void Update()
    {
        // minDistanceForHighlight = 20f;
        float distance = Vector3.Distance(jugador.transform.position, transform.position);

        if (distance <= minDistanceForHighlight)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(jugador.tag))
        {
            if (Input.GetButtonDown("Fire1") && !flag)
            {
                flag = true;
                StartCoroutine(AgregarInverntarioEnumerator());
            }
        }
    }

    IEnumerator AgregarInverntarioEnumerator()
    {
        GameObject objeto_copia = Instantiate(gameObject, transform.position, transform.rotation);
        objeto_copia.SetActive(false);

        yield return new WaitForSeconds(tiempoParaRecojer);

        jugador.GetComponent<Inventario>().agregarAInventario(objeto_copia);
        // Debug.Log(string.Format("pieza: {0} nave: {1}", nombreDePieza, nave));
        nave.GetComponent<armarNave>().armar(nombreDePieza);

        // AGREGADO POR JUAN
        try
        {
            int piezasColectadas = jugador.GetComponent<Inventario>().getPiezasColectadasConteo();
            Debug.Log(string.Format("{0} de {1}", piezasColectadas, 18));
            if (nave.GetComponent<armarNave>().estaCompleta())
            {
                winState = true;
                stopAllAudio();
                jugador.GetComponent<movement>().enabled = false;
                jugador.GetComponent<Animator>().SetBool("dance", true);
                winTheme.Play();
                winTheme.volume = 1.0f;

                Debug.Log("HA GANADO");
            }
        }
        catch (UnityException e)
        {
            Debug.LogError(e.Message);
        }



        Destroy(gameObject, 0.10f);
    }

    void stopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }
}
