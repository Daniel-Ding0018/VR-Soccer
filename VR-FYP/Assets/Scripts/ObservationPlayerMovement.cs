using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservationPlayerMovement : PlayerMovement
{


    public GameObject LaceKickingTimelineController;

    protected override void handleShoot()
    {
        if (sideKickPressed)
        {
            Debug.Log("Side Kick Pressed");
            RightShoe.GetComponent<KickBall>().enableSideKicking();
            animator.SetBool(isSideKickingHash, true);
        }

        if (!sideKickPressed)
        {
            RightShoe.GetComponent<KickBall>().disableSideKicking();
            animator.SetBool(isSideKickingHash, false);
        }

        if (laceKickPressed)
        {
            Debug.Log("Lace Kick Pressed");
            LaceKickingTimelineController.GetComponent<LaceKickingTimelineScript>().play();
            RightShoe.GetComponent<KickBall>().enableLaceKicking();
            animator.SetBool(isLaceKickingHash, true);
        }

        if (!laceKickPressed)
        {
            RightShoe.GetComponent<KickBall>().disableLaceKicking();
            animator.SetBool(isLaceKickingHash, false);
        }
    }
}
