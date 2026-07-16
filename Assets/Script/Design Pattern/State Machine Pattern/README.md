# Core State Machine
* Một hệ thống State Machine giúp phân tách các hành động của nhân vật thành các script riêng giúp dễ quản lý và chỉnh sửa.
* A State Machine system helps separate a character's actions into individual scripts, making them easier to manage and edit.
## 📦 Yêu cầu hệ thống (Requirements)
* Unity 6000.3 trở lên.
* Unity 6000.3 or higher

## 📖 Cách sử dụng cơ bản (Basic Usage)

#### Package bao gồm 4 script (Pakage includes 4 script): 
* State: Quản lý các hành vi của trạng thái.
* StateMachine: Đóng vai trò đầu não để phân phối các trạng thái.
* PlayerBaseState: Kế thừa State, quản lý hành vi mang tính riêng biệt của object Player.
* PlayerStateMachine: Kế thừa StateMachine, tạo một script quản lý lớn nhất cho object Player.
* LocomotionState: Script quản lý hành động: Idle, Walk, Run.
******************************************************************************
* State: Manages the behaviors of a state.
* StateMachine: Acts as the brain to distribute states.
* PlayerBaseState: Inherits from State, manages behaviors specific to the Player object.
* PlayerStateMachine: Inherits from StateMachine, creates the main management script for the Player object.
* LocomotionState: Script that manages actions: Idle, Walk, Run.
#### Luồng hoạt động (Workflow):
* State -> PlayerBaseState => Chứa script di chuyển,...
* StateMachine -> PlayerStateMachine => References các biến như tốc độ, Animator,... Gọi SwitchState(new LocomotionState(this)) ở hàm Start để bắt đầu với Locomotion State.
* PlayerBaseState -> LocomotionState => Gọi animation, script di chuyển từ PlayerBaseState và SwitchState.
* ******************************************************************************
* State -> PlayerBaseState => Contains movement scripts,...
* StateMachine -> PlayerStateMachine => References variables like speed, Animator,... Call SwitchState(new LocomotionState(this)) in the Start function to begin with the Locomotion State.
* PlayerBaseState -> LocomotionState => Calls animation, movement script from PlayerBaseState and SwitchState.

