# GameProjectDemo

**（置顶）BUG警告：**

角色点击Attack或Skill后仍可以点击友方角色，随后再点击目标会导致报错！

**本次更新：已完成相机运动。**

新加入了自动追踪可操作角色以及相机抖动。

当点击“EndTurn”时自动切换到下一个可操作角色。（这里在EndTurnButton里添加了一行。话说为什么不用按钮自带的OnClick？）

使用简单的噪声做了个相机抖动。

追踪和抖动都是空开的函数，可以使用Camera.main.GetComponent\<CameraController>().Tracing/CameraShake访问。

其中追踪具有回调用于在动画结束后使用。

抖动目前应用于技能命中瞬间，由特效生成时直接调用。（弓箭手因为没有使用我的技能特效所以没有抖动效果）

