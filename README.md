# FallingBlock（接球小游戏）

方块从天上随机掉落，操作底部的挡板左右移动去接住它们。

> 配套教程：`F:\CSharp\2048\docs\unity\基础篇\08-综合练习-小游戏.md`
>
> 该项目使用的 Unity 版本：**2022.3.62f3（LTS）**，模板 **2D (Built-In Render Pipeline)**

## 1. 玩法规则

| 事件 | 结果 |
| --- | --- |
| 接住掉落物 | 分数 **+1** |
| 漏掉掉落物（掉出屏幕底部） | 生命 **-1** |
| 生命归零 | 游戏结束，弹出结算面板 |
| 时间推移 | 掉落间隔逐渐变短、下落速度逐渐加快 |

**初始状态**：分数 0、生命 3、最高分从 `PlayerPrefs` 读取（首次为 0）。

**历史最高**：超过记录时立即写入 `PlayerPrefs`（键名 `catch_best`）。

## 2. 操作

| 输入 | 作用 |
| --- | --- |
| `A` / `D` / `←` / `→` | 左右移动挡板 |
| 鼠标点击"再来一局" | 重开 |
| `Esc`（可选扩展） | 暂停 / 继续 |

## 3. 世界尺寸约定

| 项 | 值 |
| --- | --- |
| 摄像机 | Orthographic，`Size = 6`（可见高度 12 个单位，宽度约 21） |
| 可视范围 | x ≈ ±10.5，y ≈ ±6 |
| 玩家活动范围 | x ∈ [-7.2, 7.2]，y = -4.5 |
| 生成高度 | y = 6.5 |
| 判定"漏掉"的高度 | y < -6.5 |

## 4. 场景结构

```
SampleScene
├── Main Camera             Camera（正交，Size 6，Position (0,0,-10)）
├── Background              SpriteRenderer（bg.png，Scale (22,12,1)，Order in Layer -10）
├── Player                  SpriteRenderer + BoxCollider2D(Is Trigger) + PlayerController
├── Spawner                 Spawner 脚本（空对象，Position (0,0,0)）
├── Game                    AudioSource + GameManager
└── Canvas                  Canvas + CanvasScaler + GraphicRaycaster
    ├── ScoreText           TextMeshProUGUI（左上角）
    ├── LivesText           TextMeshProUGUI（右上角）
    ├── BestText            TextMeshProUGUI（左上角第二行）
    └── GameOverPanel       Panel（默认隐藏）
        ├── GameOverText    TextMeshProUGUI
        └── RestartButton   Button
```

另有 `Prefabs/Faller.prefab`（掉落物模板，不在场景里，靠代码生成）。

## 5. 组件清单

| 对象 | 组件 | 关键设置 |
| --- | --- | --- |
| Main Camera | Camera | `Orthographic`、`Size = 6`、Position `(0,0,-10)` |
| Background | SpriteRenderer | `bg.png`、`Order in Layer = -10`、Scale `(22,12,1)` |
| Player | SpriteRenderer | `player.png` |
| | Box Collider 2D | **`Is Trigger` 勾上**、Size `(1,0.25)` 左右（按图片比例调） |
| | PlayerController | speed 9、limitX 7.2 |
| Spawner | Spawner | 拖入 `Faller.prefab`、spawnY 6.5、rangeX 7、startInterval 1.0、minInterval 0.32、rampSeconds 60 |
| Faller（Prefab） | SpriteRenderer | `faller.png` |
| | Rigidbody 2D | `Dynamic`、`Gravity Scale = 1`、**`Constraints → Freeze Rotation Z`** |
| | Box Collider 2D | **`Is Trigger` 勾上** |
| | Faller | 掉到 y < -6.5 时通知游戏并销毁 |
| Game | Audio Source | **取消 `Play On Awake`** |
| | GameManager | 引用见下 |
| Canvas | Canvas | `Screen Space - Overlay` |
| | Canvas Scaler | `Scale With Screen Size`、参考分辨率 1920×1080 |
| | Graphic Raycaster | 默认即可 |

**GameManager 需要拖的引用**：

| 字段 | 拖什么 |
| --- | --- |
| `scoreText` | Canvas/ScoreText |
| `livesText` | Canvas/LivesText |
| `bestText` | Canvas/BestText |
| `gameOverPanel` | Canvas/GameOverPanel |
| `gameOverText` | Canvas/GameOverPanel/GameOverText |
| `restartButton` | Canvas/GameOverPanel/RestartButton |
| `audioSource` | Game 自身 |
| `catchClip` / `missClip` | 可选音效 |

## 6. 脚本清单

| 脚本 | 职责 | 关键成员 |
| --- | --- | --- |
| `PlayerController` | 挡板移动 + 接住掉落物 | `speed`、`limitX`、`OnTriggerEnter2D` |
| `Faller` | 下落物：出界时通知游戏并销毁 | `BottomY = -6.5f`、`OnMiss()` |
| `Spawner` | 协程定时生成掉落物，难度递增 | `fallerPrefab`、`spawnY`、`startInterval`、`minInterval`、`rampSeconds` |
| `GameManager` | 分数 / 生命 / 局状态 / UI 刷新 / 最高分 | `Instance`、`IsPlaying`、`StartGame`、`OnCatch`、`OnMiss` |

**调用关系**：

```
PlayerController.OnTriggerEnter2D ──► GameManager.OnCatch()
Faller.Update（出界）              ──► GameManager.OnMiss()
Spawner.SpawnLoop（协程）          ──► Instantiate(fallerPrefab)
GameManager.EndGame                ──► 显示 GameOverPanel
RestartButton.onClick              ──► GameManager.StartGame()
```

## 7. 图片资源

全部在 `Assets/Images/`，**建议统一把 `Pixels Per Unit` 改成 256**（选中图片 → Inspector → PPU → Apply），这样 256×256 的图正好是 1 个世界单位。

| 文件 | 尺寸 | 用途 | 建议 Scale |
| --- | --- | --- | --- |
| `bg.png` | 256×256 | 背景（深蓝灰） | `(22, 12, 1)` |
| `player.png` | 512×128 | 挡板（青色，带顶部高光） | `(1, 1, 1)`（PPU 256 时约 2×0.5 单位） |
| `faller.png` | 256×256 | 普通掉落物（暖橙） | `(0.7, 0.7, 1)` |
| `faller_gold.png` | 256×256 | **扩展**：金色道具（+3 分） | `(0.7, 0.7, 1)` |
| `faller_bomb.png` | 256×256 | **扩展**：炸弹（扣 1 条命） | `(0.7, 0.7, 1)` |

全部为 PNG 透明背景、圆角矩形；玩家和掉落物带一条顶部高光。

## 8. 需要达到的效果

1. 运行后橙色方块从顶部随机位置落下
2. 挡板跟随 A/D 或方向键左右移动，到边界停住
3. 接住方块：分数 +1、方块消失
4. 漏掉方块：生命 -1、方块在屏幕底部消失
5. 时间越久，方块掉得越快、间隔越短
6. 生命归零：弹出结算面板，显示本局分数与历史最高
7. 点"再来一局"：分数归零、生命恢复、面板隐藏、场上残留方块被清空
8. 退出 Play 再进入：最高分仍然保留

## 9. 常见故障对照

| 现象 | 原因 |
| --- | --- |
| 方块掉下来但接不住 | 挡板没勾 `Is Trigger`；或掉落物没有 `Rigidbody2D`（触发检测需要至少一方有刚体） |
| 一接住分数狂涨 | 忘了 `Destroy(other.gameObject)`，或把逻辑写在了 `OnTriggerStay2D` |
| 方块边掉边翻滚 | `Rigidbody2D` 的 `Constraints` 没勾 `Freeze Rotation Z` |
| 报 `NullReferenceException` 在 `GameManager` | Inspector 里的引用没拖全 |
| 方块是黑的 | 用了 URP 模板但没加 `Global Light 2D`（本项目应为 Built-In 模板） |
| 按钮点不动 | 场景里缺 `EventSystem`；或按钮 `Image` 的 `Raycast Target` 没勾 |

## 10. 扩展练习（做完基础版再挑）

1. **暂停**：`Esc` 切换 `Time.timeScale` 0/1（UI 动画要用 `WaitForSecondsRealtime`）
2. **金色道具**：用 `faller_gold.png` 做一个 +3 分的 Prefab，接住时加 3 分
3. **炸弹**：用 `faller_bomb.png`，接住扣 1 条命
4. **难度可视化**：用 `OnDrawGizmos` 画出生成范围
5. **换玩法**：把"接球"改成"躲球"（碰到就扣命），只需要改 `OnTriggerEnter2D` 里的几行
