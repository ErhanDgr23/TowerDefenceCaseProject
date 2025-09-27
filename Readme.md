# Tower Defence Prototype

## 🎯 Proje Amacı
Bu proje, 3D bir ortamda 2D sprite’lar (billboard tekniği) kullanılarak basit bir **Tower Defence prototipi** geliştirmeyi amaçlamaktadır. 
Odağımız, teknik yapı ve temel oyun döngüsünü kurmaktır.

## 🕹️ Oynanış
- Oyuncu WASD ile hareket eder.
- En yakındaki düşmana otomatik saldırır (menzilli).
- Düşmanlar dalga dalga gelir ve belirlenen yolu takip eder.
- Düşmanlar bitiş noktasına ulaştığında base’in HP’si azalır.
- Belirli dalgalarda boss düşman çıkar **(5. wave)** (toplamda 5 wave var oyunda).
- Oyuncu ölürse veya base yok olursa oyun biter.

## ⚙️ Kullanılan Teknolojiler
- **Oyun Motoru:** Unity (v2022.3.27f1)  
- **Dil:** Unity C#  
- **Platform:** PC (Windows)  

## 📂 Proje Yapısı
- `Assets/` → Oyun içi kodlar, prefab’lar ve assetler  
- `Scripts/` → C# kodları  
- `Prefabs/` → düşman  
- `Scenes/` → Ana sahne
- `VFX/` → Partikül ve görsel efektler  

## 🖼️ Kullanılan Assetler
- Karakter ve düşman sprite’ları: [Undead Survivor Assets Pack](https://assetstore.unity.com/packages/2d/undead-survivor-assets-pack-238068)  
- Yol ve çevre: [Undead Survivor Assets Pack](https://assetstore.unity.com/packages/2d/undead-survivor-assets-pack-238068)  
- VFX: [itch.io,blood](https://xyezawr.itch.io/gif-free-pixel-effects-pack-5-blood-effects) [itch.io,VFX](https://bdragon1727.itch.io/750-effect-and-fx-pixel-all)
- UI Menu: [itch.io,UI](https://bdragon1727.itch.io/basic-pixel-health-bar-and-scroll-bar)

## 📌 Varsayımlar ve Notlar
- Asset’ler **placeholder** olarak kullanılmıştır.
- Odak noktası **oynanabilir prototip** oluşturmaktır, görsellik ikinci plandadır.
- Performans testi için ortalama 1200-1500 düşman animasyonu Test edilmiştir (Custom Animator scripti yazılıp Unity Animator kullanılmamıştır).

## 🚀 Nasıl Çalıştırılır
1. Bu projeyi indirin veya klonlayın:  
   ```bash
   git clone https://github.com/ErhanDgr23/TowerDefenceCaseProject
