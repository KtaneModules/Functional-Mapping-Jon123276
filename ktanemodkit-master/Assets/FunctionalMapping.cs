using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using System.Text.RegularExpressions;
using Rnd = UnityEngine.Random;

public class FunctionalMapping : MonoBehaviour {
	public KMBombModule Module;
	public KMBombInfo Bomb;
	public KMAudio Audio;
	public TextMesh stage;
	public KMSelectable[] FunctionOpts;
	private static int moduleId = 0;
	bool moduleSolved;
	bool pressedButton = false;
	public GameObject[] AImage;
	public GameObject[] BImage;
	public GameObject[] CImage;
	public GameObject[] DImage;
	public GameObject[] Images; // Used to deactivate un-needed codomains.
	public GameObject[] Functions; // Used to deactivate the buttons.
	int randomImage1 = 0;
	int randomImage2 = 0;
	int randomImage3 = 0;
	int randomImage4 = 0;
	int[] arrows = {0, 1, 2, 3};
	string correctOp;
	string chosenOp;
	// Pairs of Elements
	struct Pair{
		public char preImage;
		public int image;
		public Pair(char preImage, int image){
			this.preImage = preImage;
			this.image = image;
		}
	};
	List<Pair> Pairs = new List<Pair>();
	void Awake(){
		moduleId++;
		giveStuffForMe ();
	}
	void Start () {
		FunctionOpts [0].OnInteract += delegate {
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, FunctionOpts[0].transform);
			chosenOp = "Surjection";
			answer();
			return false;
		};
		FunctionOpts [1].OnInteract += delegate {
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, FunctionOpts[1].transform);
			chosenOp = "Bijection";
			answer();
			return false;
		};
	}
	void checkOp(){
		if (randomImage1 != randomImage2 && randomImage1 != randomImage3 && randomImage1 != randomImage4 && randomImage2 != randomImage3 && randomImage2 != randomImage4 && randomImage3 != randomImage4){
			correctOp = "Bijection";
		} else {
			correctOp = "Surjection";
		}
		Debug.LogFormat ("[Functional Mapping #{0}] The mapping is a {1}.", moduleId, correctOp);
	}
	void giveArrows(){
		AImage [randomImage1].SetActive (true);
		BImage [randomImage2].SetActive (true);
		CImage [randomImage3].SetActive (true);
		DImage [randomImage4].SetActive (true);
	}
	void answer(){
		Debug.LogFormat ("[Functional Mapping #{0}] The chosen mapping is {1}", moduleId, chosenOp);
		if (chosenOp.Equals (correctOp)) {
			Pairs.Clear();
			Pairs.TrimExcess ();
			stage.text = (int.Parse(stage.text)+1).ToString();
			Debug.Log (stage.text);
			if (stage.text == 11.ToString()) {
				Module.HandlePass ();
				Functions [0].SetActive (false);
				Functions [1].SetActive (false);
			}
			giveStuffForMe ();
		} else {
			Pairs.Clear();
			Pairs.TrimExcess ();
			Module.HandleStrike ();
			stage.text = 1.ToString();
			giveStuffForMe ();
		}

	}
	void giveStuffForMe(){
		stopGivingDouble ();
		giveArrows ();
		givePairs ();
		checkOp ();
	}
	void stopGivingDouble(){
		Images[0].SetActive(true);
		Images[1].SetActive(true);
		Images[2].SetActive(true);
		Images[3].SetActive(true);
		for (int i = 0; i < AImage.Length; i++) {
			AImage [i].SetActive (false);
			BImage [i].SetActive (false);
			CImage [i].SetActive (false);
			DImage [i].SetActive (false);
		}
		randomImage1 = Rnd.Range(0, 1000)%4;
		randomImage2 = Rnd.Range(0, 1000)%4;
		randomImage3 = Rnd.Range(0, 1000)%4;
		randomImage4 = Rnd.Range(0, 1000)%4;
		Debug.Log (randomImage1);
		Debug.Log (randomImage2);
		Debug.Log (randomImage3);
		Debug.Log (randomImage4);
		arrows[0] = randomImage1;
		arrows[1] = randomImage2;
		arrows[2] = randomImage3;
		arrows[3] = randomImage4;
		for (int i = 0; i < 4; i++) {
			if (randomImage1 != i && randomImage2 != i && randomImage3 != i && randomImage4 != i) {
				Images[i].SetActive(false);
			}
		}
	}
	void givePairs(){
		Pairs.Add (new Pair ('A', randomImage1+1));
		Pairs.Add (new Pair ('B', randomImage2+1));
		Pairs.Add (new Pair ('C', randomImage3+1));
		Pairs.Add (new Pair ('D', randomImage4+1));
		for (int i = 0; i < Pairs.Capacity; i++) {
			Debug.LogFormat("[Functional Mapping #{0}] {1}, {2}", moduleId,Pairs [i].preImage, Pairs [i].image);
		}
		Debug.Log (Pairs.Capacity);
	}
}
