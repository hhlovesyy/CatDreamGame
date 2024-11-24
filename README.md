# 程序开发日志 2024.11.23~2024.12.01

11.23

讨论出基础玩法，寻找猫猫免费素材和动画

- [x] 玩法：制作Slider的变化逻辑，可以通过外面的事件系统调用；
- [x] 玩法：可以与可交互物体交互，将物体从空中摔下来，摔到地上会发送slider变化的事件。
- [x] 更新：更新了一下UI框架的camera screen问题，现在可以显示场景了（overlay）
- [x] 玩法：制作Slider的改变upperbound的逻辑；
- [x] 玩法：完成了猫猫的基础移动+跳跃逻辑；



11.24

- [ ] 玩法：完善游戏基础逻辑、关卡选择和胜利条件的UI显示
  - [ ] 倒计时
  - [ ] 胜利后的星级显示（依据提前完成的时间决定本关的星级）
  - [ ] 关卡选择和是否能够解锁（用XML+存档来体现），关卡可以用自定义ScrollView来实现
  - [ ] 界面完善逻辑，包括设置界面，游戏暂停界面，选关界面，游戏关卡通关/失败界面
- [ ] 玩法：猫猫与物品的交互逻辑完善，依据不同的物体重量有不同的交互方式
- [ ] 交互：完善UI设计，可交互的物品有对应的按键提示
- [ ] 优化：镜头完善
- [ ] 优化：比较重的物体在撞击的时候会滑走（手感不好），可能后面需要特殊处理一下这个问题
- [x] 把睡眠条的逻辑进一步完善，包含上限值和恢复速度
- [x] 把物品策划表的字段加上，研究一下角色controller和物品重量的关系

