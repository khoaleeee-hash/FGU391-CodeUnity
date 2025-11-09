using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    public Animator playerAnim;
    public bool isFishing;
    public bool poleBack;
    public bool throwBobber;
    public Transform fishingPoint;
    public GameObject bobber;

    public float targetTime = 0.0f;
    public float savedTargetTime;
    public float extraBobberDistance;

    public GameObject fishGame;

    public float timeTillCatch = 0.0f;
    public bool winnerAnim;

    private Vector3 originalFishingPointPos;

    void Start()
    {
        isFishing = false;
        fishGame.SetActive(false);
        throwBobber = false;
        targetTime = 0.0f;
        savedTargetTime = 0.0f;
        extraBobberDistance = 0.0f;
        originalFishingPointPos = fishingPoint.position;
    }

    void Update()
    {
        // Kéo cần
        if (Input.GetKeyDown(KeyCode.Space) && !isFishing && !winnerAnim)
        {
            poleBack = true;
        }

        // Câu cá
        if (isFishing)
        {
            timeTillCatch += Time.deltaTime;
            if (timeTillCatch >= 3)
            {
                fishGame.SetActive(true);
            }
        }

        // Thả cần → ném bobber
        if (Input.GetKeyUp(KeyCode.Space) && !isFishing && !winnerAnim)
        {
            poleBack = false;
            isFishing = true;
            throwBobber = true;

            extraBobberDistance = (targetTime >= 3) ? 3 : targetTime;

            // Tính vị trí mới cho fishingPoint trước khi ném
            fishingPoint.position = originalFishingPointPos + new Vector3(extraBobberDistance, 0, 0);
        }

        // Animation kéo cần
        if (poleBack)
        {
            if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("playerSwingBack"))
                playerAnim.Play("playerSwingBack");

            savedTargetTime = targetTime;
            targetTime += Time.deltaTime;
        }

        // Ném bobber và animation câu
        if (isFishing)
        {
            if (throwBobber)
            {
                Instantiate(bobber, fishingPoint.position, fishingPoint.rotation); // Không parent player
                fishingPoint.position = originalFishingPointPos; // reset
                throwBobber = false;
                targetTime = 0.0f;
                savedTargetTime = 0.0f;
                extraBobberDistance = 0.0f;
            }

            if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("playerFishing"))
                playerAnim.Play("playerFishing");
        }

        // Hủy câu
        if (Input.GetKeyDown(KeyCode.C) && timeTillCatch <= 3)
        {
            playerAnim.Play("playerStill");
            poleBack = false;
            throwBobber = false;
            isFishing = false;
            timeTillCatch = 0;
        }
    }

    public void fishGameWon()
    {
        playerAnim.Play("playerWonFish");
        fishGame.SetActive(false);
        poleBack = false;
        throwBobber = false;
        isFishing = false;
        timeTillCatch = 0;
    }

    public void fishGameLossed()
    {
        playerAnim.Play("playerStill");
        fishGame.SetActive(false);
        poleBack = false;
        throwBobber = false;
        isFishing = false;
        timeTillCatch = 0;
    }
}
