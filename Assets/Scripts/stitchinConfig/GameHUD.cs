using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class GameHUD : MonoBehaviour {
  public Sprite fullHeartSprite;
  public Sprite halfHeartSprite;
  public Sprite emptyHeartSprite;

  private float localHealth;

  private Vector3 heartsOffset = new Vector3(-170, 90, 0);
  private List<SpriteRenderer> hearts = new List<SpriteRenderer>();
  private int currentBubbleHeart = 0;
  private bool reverseBubbleHeart = false;

  void Awake () {
    localHealth = Stitch.katHealth;

    for (int i = 0; i < Stitch.heartCount; ++i) {
      hearts.Add(CreateHeart(i*15, GetSpriteForIndex(i)));
    }
    StartCoroutine("BubbleHearts");
  }

  void Update () {
    for (int i = 0; i < hearts.Count; ++i) {
      if (hearts[i].transform.localScale.x > 0.38f)
        hearts[i].transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
    }
  }

  public SpriteRenderer GetHeart(int index) {
    return hearts[index];
  }
  public int GetNewHeartIndex() {
    return (int)(Stitch.katHealth + 0.25);
  }

  public void AddHeart(int index) {
    currentBubbleHeart = index;
    hearts[currentBubbleHeart].transform.localScale += new Vector3(0.12f, 0.12f, 0.12f);
    hearts[currentBubbleHeart].sprite = fullHeartSprite;
    this.localHealth = Math.Min(this.localHealth + 1, Stitch.heartCount);

    if (hearts.Count - 1 != currentBubbleHeart)
      hearts[currentBubbleHeart + 1].sprite = GetSpriteForIndex(currentBubbleHeart + 1);

    reverseBubbleHeart = true;
    StopCoroutine("BubbleHearts");
    StartCoroutine("BubbleHearts");
  }

  public void LoseHealth(float amount) {
    Stitch.katHealth -= amount;
    this.localHealth -= amount;
    currentBubbleHeart = GetNewHeartIndex();
    hearts[currentBubbleHeart].transform.localScale += new Vector3(0.15f, 0.15f, 0.15f);

    Game.CreateParticle("LoseHeart", hearts[currentBubbleHeart].gameObject.transform.position).transform.parent = transform;
    RefreshHearts();

    reverseBubbleHeart = true;
    StopCoroutine("BubbleHearts");
    StartCoroutine("BubbleHearts");
  }

  private void RefreshHearts() {
    for (int i = 0; i < hearts.Count; ++i) {
      hearts[i].sprite = GetSpriteForIndex(i);
    }
  }

  private SpriteRenderer CreateHeart(float x, Sprite spr) {
    GameObject heart = new GameObject();
    heart.transform.parent = transform;
    heart.transform.localPosition = heartsOffset + new Vector3(x, 0, 0);
    heart.transform.localScale = new Vector3(0.38f, 0.38f, 0.38f);

    SpriteRenderer s = heart.AddComponent<SpriteRenderer>();
    s.sprite = spr;
    s.sortingOrder = 20000;
    return s;
  }

  private Sprite GetSpriteForIndex(int i) {
    if (this.localHealth >= i + 0.75f)
      return fullHeartSprite;
    else if (this.localHealth >= i + 0.25f)
      return halfHeartSprite;
    else
      return emptyHeartSprite;
  }

  /*************************************************************
   * CO-ROUTINES
   ************************************************************/

  protected virtual IEnumerator BubbleHearts() {
    while (true) {
      hearts[currentBubbleHeart].transform.localScale += new Vector3(0.08f, 0.08f, 0.08f);

      if (reverseBubbleHeart)
        currentBubbleHeart--;
      else
        currentBubbleHeart++;

      if (currentBubbleHeart >= Math.Min(this.localHealth, hearts.Count)) {
        currentBubbleHeart = 0;
        yield return new WaitForSeconds(3);
      }
      else if (currentBubbleHeart < 0) {
        currentBubbleHeart = 0;
        reverseBubbleHeart = false;
        yield return new WaitForSeconds(3);
      }
      else
        yield return new WaitForSeconds(reverseBubbleHeart ? 0.01f : 0.1f);
    }
  }

}
