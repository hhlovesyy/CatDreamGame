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
  - [x] 倒计时
  - [x] 胜利后的星级显示（依据提前完成的时间决定本关的星级）
  - [x] 关卡选择和是否能够解锁（用XML+存档来体现），关卡可以用自定义ScrollView来实现
  - [x] 界面完善逻辑，包括设置界面，游戏暂停界面，选关界面，游戏关卡通关界面
  - [ ] 失败界面，以及继续完善逻辑，胜利之后返回主界面的逻辑还没做
- [ ] 玩法：猫猫与物品的交互逻辑完善，依据不同的物体重量有不同的交互方式
- [ ] 交互：完善UI设计，可交互的物品有对应的按键提示
- [ ] 优化：镜头完善
- [ ] 优化：比较重的物体在撞击的时候会滑走（手感不好），可能后面需要特殊处理一下这个问题
- [x] 把睡眠条的逻辑进一步完善，包含上限值和恢复速度
- [x] 把物品策划表的字段加上，研究一下角色controller和物品重量的关系
- [x] 暂停逻辑中，“返回主菜单”涉及到一些资源的卸载，场景关卡的重加载什么的，已完成；



11.25

- [x] 完成物体类的基本框架，写几种可复用的逻辑；
- [x] 测试关卡的切换，状态切换/重置的相关逻辑；
- [x] 做了一个模拟的易拉罐的效果，先这样（level1预制体里面有）； 



11.26

- [ ] 完成音频类的相关代码，让游戏可以高效播放各种音效；
- [ ] 完成策划表中与持续发声有关的逻辑，比如收音机这种；
- [ ] 完成电脑的逻辑，按住会启动，长按会报错；
- [ ] 替换2D美术制作的素材；
- [ ] 完成
