using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinking_UI : MonoBehaviour
{
   [SerializeField] private Animator blinking_effect;
   [SerializeField] private Image blinking_jump;
   [SerializeField] private GameObject Player;

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag(("Player")))
      {
         blinking_effect.enabled = true;
      }
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      if (other.CompareTag(("Player")))
      {
         blinking_effect.enabled = false;
         blinking_jump.enabled = false;
      }
   }
}
