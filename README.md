# card-hunter

**《怪物猎人》太刀主题的卡牌 Roguelike 战棋游戏** · Unity 2D

把太刀的招式派生表做成卡组，把狩猎场做成六边形棋盘。用卡牌决定「这一刀往哪砍、砍多远、砍完能不能连」。

```text
委托板接委托 → 路线图选节点 → 六边形战棋出牌狩猎 → 赚钱 / 加卡 / 改卡组 → 下一个委托
```

## 核心系统

### ① 气刃与双状态机

太刀的灵魂是气刃槽和招式派生，这里被完整搬进了卡牌规则：

| 机制 | 数值 / 行为 |
|---|---|
| 气刃槽 | 8 格，卡牌可 `+气刃` / `-气刃` |
| 刃级 | 3 档（白 → 黄 → 红），刃级越高伤害倍率越高 |
| 自由态 / 连携态 | 卡牌可标记「仅限某态可用」与「使用后进入某态」，构成连招链 |

37 张卡全部对应真实太刀招式：直斩、踏步斩、气刃斩 1/2、气刃大回旋、见切、居合拔刀斩、看破斩·旋、气刃兜割、飞翔爪攻击、炼气解放无双斩、圆月……

每张卡由多个词条所定义：

```csharp
Cost / Attack(Vector2Int) / Defence / Wound      // 费用、伤害区间、格挡、伤口
Move[] / MoveLength / AttackDirection[]           // 移动方向、步长、攻击方向
AttackRange[] / AttackLength                      // 攻击范围形状、贯穿长度
Derivation / OnlyLState / EnterState              // 派生卡、状态限制、状态切换
DeltaBladeNum / DeltaBladeLevel / DeltaCost       // 回气刃、升刃级、回费
Buff[] / DrawCard / Consumption / Nothingness     // Buff、抽牌、消耗、虚无
Sequence                                          // 先攻后移 / 先移后攻
```

### ② 六边形战棋

- **7 × 7** hex 网格，轴向坐标，**6 向**移动（`(1,0) (0,1) (-1,1) …`）
- 障碍物 15% 生成率、上限 10 个；生成后走**连通性校验**，避免地图被切成孤岛
- 8 种地图要素（20% 生成率）：草丛、陷阱、青蛙、毒草、耐力虫、岩浆、电球、冰柱
- 卡牌的「移动 + 攻击」是分离的两段动作，出牌时分别染色预览落点与击打范围

### ③ 敌人技能预演

战棋的公平性来源：敌人下一招**提前可视**。

- `EnemySkillSystem` 维护 `nextSkillID` 和 `nextskillpos`，在棋盘上高亮红色预警区
- 玩家移动后触发 `ChangeSkillinRealtime()` —— 按新站位**实时重算**技能落点与覆盖范围，玩家可以走出预警区
- 技能执行按帧分段（`ExecuteCurrentSkill(cut)`），配合伤害判定、上甲、治疗、位移等子效果
- `EnemyAIController` 用 A\* 寻路接近玩家，`direction` 维护朝向，脱离视野时随机游走

## 工程实现

| 关注点 | 做法 |
|---|---|
| 卡牌数据 | `GameConfig : ScriptableObject` 集中配置表，`Card` 组件按 ID 取属性，数据与表现分离 |
| 卡牌对象 | `CardPool` 对象池，`GetCard` / `ReturnCard` 复用，避免战斗中频繁 Instantiate |
| 手牌布局 | 弧形排布（`arcHeight` + `maxRotation`），随张数动态计算位置与倾斜 |
| 出牌交互 | `CardController` 实现 `IBeginDragHandler / IDragHandler / IEndDragHandler`，拖拽出牌 + Hover 抬升 |
| 战斗流程 | `BattleManager`（约 1000 行）持有 `BattleState` 状态机，统一调度玩家回合、敌人回合、技能结算 |
| 随机生成 | `RogueMod` 负责委托抽取、事件投放、路线节点分配 |
| UI / 字体 | TextMesh Pro + 中文字体位数集（微软雅黑 / 华文细黑） |

## Roguelike 层

- **委托（Commission）**：随机怪物 × 难度 × 地形（森林 / 荒漠 / 火山）× 报酬
- **事件（Event → Choice）**：每个选项可修改卡组、触发战斗、增减金币与生命上限、给予卡牌或装备
- **路线（EventArrangement）**：节点数 = `2 × 委托数 - 1`，首节点固定，其余按上限 2 事件/节点随机投放
- **卡池稀有度**：normal / rare / gold 三档独立权重表
- 商店、营地、牌组编辑三套外围界面

## 运行

Unity **2022.3.56f1**，2D 模板 + TextMesh Pro。

```bash
git clone https://github.com/fivekilo/card-hunter.git
# Unity Hub → Add project from disk → 选择 card-hunter/card-hunter/
```

场景：`Assets/Scenes/SampleScene.unity`（战斗）、`Assets/Scenes/WorldMap.unity`（世界地图）

## 目录

```text
card-hunter/Assets/
├─ Config/config.cs        # 全局配置：卡牌表、敌技表、数值常量
├─ OuterScripts/           # Roguelike 外层：地图路线、事件、商店、营地、牌组
├─ Scenes/                 # 战斗场景 / 世界地图场景
├─ Resources/              # 卡面、怪物立绘、招式特效、音效、prefab
├─ Card*.cs                # 卡牌数据、控制器、对象池、手牌管理
├─ BattleManager.cs        # 战斗主循环与状态机
├─ MapManager.cs / Map.cs  # 六边形棋盘、连通性、范围计算
├─ EnemyAIController.cs    # 敌人寻路与朝向
└─ EnemySkillSystem.cs     # 敌人技能预警与结算
```

## 版权说明

项目中的怪物立绘、招式特效、音效素材均来自 **CAPCOM《怪物猎人》**，版权归原厂所有。本项目为个人学习与研究性质的同人作品，**不作任何商业用途**，也不分发游戏本体资源用于盈利。
