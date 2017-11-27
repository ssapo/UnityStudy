using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

// this class will take care of switching turns and counting down time until the turn expires
public class TurnManager : MonoBehaviour
{
	// PUBLIC FIELDS
	public CardAsset CoinCard;

	// for Singleton Pattern
	public static TurnManager Instance;

	// PRIVATE FIELDS
	// reference to a timer to measure 
	private RopeTimer timer;

	// PROPERTIES
	private Player _whoseTurn;
	public Player whoseTurn
	{
		get
		{
			return _whoseTurn;
		}

		set
		{
			_whoseTurn = value;
			timer.StartTimer();

			GlobalSettings.Instance.EnableEndTurnButtonOnStart(_whoseTurn);

			var tm = whoseTurn.GetComponent<TurnMaker>();
			// player`s method OnTurnStart() will be called in tm.OnTurnStart();
			tm.OnTurnStart();
			if (tm is PlayerTurnMaker)
			{
				whoseTurn.HighlightPlayableCards();

			}
			// remove highlights for opponent.
			whoseTurn.otherPlayer.HighlightPlayableCards(true);
		}
	}

	// METHODS
	private void Awake()
	{
		Instance = this;
		timer = GetComponent<RopeTimer>();
	}

	private void Start()
	{
		OnGameStart();

	}

	public void OnGameStart()
	{
		CardLogic.CardsCreatedThisGame.Clear();
		//Debug.Log("In TurnManager.OnGameStart()");
		CreatureLogic.CreaturesCreatedThisGame.Clear();

		foreach (var p in Player.Players)
		{
			p.ManaThisTurn = 0;
			p.ManaLeft = 0;
			p.LoadCharacterInfoFromAsset();
			p.TransmitInfoAboutPlayerToVisual();
			p.PArea.PDeck.CardsInDeck = p.deck.cards.Count;
			// move both portraits to the center
			p.PArea.Portrait.transform.position = p.PArea.InitialPortraitPosition.position;
		}

		var s = DOTween.Sequence();
		s.Append(Player.Players[0].PArea.Portrait.transform.DOMoveX(Player.Players[0].PArea.PortraitPosition.position.x, 0.6f).SetRelative(false).SetEase(Ease.InQuad, 1.07f));
		s.Join(Player.Players[0].PArea.Portrait.transform.DOMoveY(Player.Players[0].PArea.PortraitPosition.position.y, 0.6f).SetRelative(false).SetEase(Ease.OutQuad, 1.07f));
		s.Join(Player.Players[0].PArea.Portrait.transform.DOMoveZ(Player.Players[0].PArea.PortraitPosition.position.z, 0.6f).SetRelative(false).SetEase(Ease.InQuad, 1.07f));
		s.Join(Player.Players[1].PArea.Portrait.transform.DOMoveX(Player.Players[1].PArea.PortraitPosition.position.x, 0.6f).SetRelative(false).SetEase(Ease.InQuad, 1.07f));
		s.Join(Player.Players[1].PArea.Portrait.transform.DOMoveY(Player.Players[1].PArea.PortraitPosition.position.y, 0.6f).SetRelative(false).SetEase(Ease.OutQuad, 1.07f));
		s.Join(Player.Players[1].PArea.Portrait.transform.DOMoveZ(Player.Players[1].PArea.PortraitPosition.position.z, 0.6f).SetRelative(false).SetEase(Ease.InQuad, 1.07f));
		s.PrependInterval(1.5f);
		s.OnComplete(() =>
			{
				// determine who starts the game.
				var rnd = Random.Range(0, 2);  // 2 is exclusive boundary
											   //Debug.Log(Player.Players.Length);
				var whoGoesFirst = Player.Players[rnd];
				//Debug.Log(whoGoesFirst);
				var whoGoesSecond = whoGoesFirst.otherPlayer;
				//Debug.Log(whoGoesSecond);

				// draw 4 cards for first player and 5 for second player
				var initDraw = 4;
				for (var i = 0; i < initDraw; i++)
				{
					// second player draws a card
					whoGoesSecond.DrawACard(true);
					// first player draws a card
					whoGoesFirst.DrawACard(true);
				}
				// add one more card to second player`s hand
				whoGoesSecond.DrawACard(true);
				//new GivePlayerACoinCommand(null, whoGoesSecond).AddToQueue();
				//whoGoesSecond.GetACardNotFromDeck(CoinCard);
				new StartATurnCommand(whoGoesFirst).AddToQueue();
			});
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			EndTurn();
		}
	}

	// FOR TEST PURPOSES ONLY
	public void EndTurnTest()
	{
		timer.StopTimer();
		timer.StartTimer();
	}

	public void EndTurn()
	{
		// stop timer
		timer.StopTimer();
		// send all commands in the end of current player`s turn
		whoseTurn.OnTurnEnd();

		new StartATurnCommand(whoseTurn.otherPlayer).AddToQueue();
	}

	public void StopTheTimer()
	{
		timer.StopTimer();
	}
}

