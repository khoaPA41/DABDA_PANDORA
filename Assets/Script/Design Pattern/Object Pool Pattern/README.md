# Core Object Pool
* Một hệ thống Object Pool gọn nhẹ và đơn giản cho Unity. Giúp tái sử dụng các GameObject (như đạn, quái vật, hiệu ứng) thay vì Instantiate/Destroy liên tục, tránh hiện tượng giật lag (Garbage Collection Spikes) trong game.
* A lightweight and simple Object Pool system for Unity. Helps reuse GameObjects (like bullets, monsters, effects) instead of constantly Instantiating/Destroying them, avoiding lag spikes (Garbage Collection Spikes) in the game.
## 📦 Yêu cầu hệ thống (Requirements)
* Unity 6000.3 trở lên.
* Unity 6000.3 or higher

## 📖 Cách sử dụng cơ bản (Basic Usage)

### 1. Khởi tạo Pool (Initialize)
```csharp
// Ví dụ (EX): Tạo một pool chứa 10 viên đạn (Create pool contains 10 bullet)
Kéo script ObjectPooling vào GameObject bất kỳ trên Hierachy.
Trên prefab bullet kéo script PooledObject để dánh dấu.
Kéo prefab bullet đó vào List<PooledObject> hiện trên Inspector.
Chỉnh số lượng trên Inspector thành 10

******************************************************************

Drag the ObjectPooling script onto any GameObject in the Hierarchy.  
On the bullet prefab, drag the PooledObject script to mark it.  
Drag that bullet prefab into the List<PooledObject> shown in the Inspector.  
Set the quantity in the Inspector to 10.