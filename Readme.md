# Tower Defence Prototype

## 🎯 Proje Amacı
Bu proje, 3D bir ortamda 2D sprite’lar (billboard tekniği) kullanılarak basit bir **Tower Defence prototipi** geliştirmeyi amaçlamaktadır. 
Odağımız, teknik yapı ve temel oyun döngüsünü kurmaktır.

## 🕹️ Oynanış
- Oyuncu WASD ile hareket eder.
- En yakındaki düşmana otomatik saldırır (yakın veya menzilli).
- Düşmanlar dalga dalga gelir ve belirlenen yolu takip eder.
- Düşmanlar bitiş noktasına ulaştığında base’in HP’si azalır.
- Belirli dalgalarda boss düşman çıkar **(5. 10. 15. wave)**.
- Oyuncu ölürse veya base yok olursa oyun biter.

## ⚙️ Kullanılan Teknolojiler
- **Oyun Motoru:** Unity (v2022.3.27f1)  
- **Dil:** Unity C#  
- **Platform:** PC (Windows)  

## 📂 Proje Yapısı
- `Assets/` → Oyun içi kodlar, prefab’lar ve assetler  
- `Scripts/` → C# kodları  
- `Prefabs/` → Oyuncu, düşman, yol  
- `Scenes/` → Ana sahne, menü sahnesi  
- `Audio/` → Ses efektleri  
- `VFX/` → Partikül ve görsel efektler  

## 🖼️ Kullanılan Assetler
- Karakter ve düşman sprite’ları: [Kenney.nl](https://kenney.nl/assets)  
- Yol ve çevre: Kenney Tower Defence Top-down Pack  
- Ses efektleri: Kenney Audio + Freesound.org  
- VFX: Unity Particle Pack  

## 📌 Varsayımlar ve Notlar
- Asset’ler **placeholder** olarak kullanılmıştır.
- Odak noktası **oynanabilir prototip** oluşturmaktır, görsellik ikinci plandadır.
- Performans testi için ortalama 1500–2500 düşman animasyonu hedeflenmiştir.  

## 🚀 Nasıl Çalıştırılır
1. Bu projeyi indirin veya klonlayın:  
   ```bash
   git clone https://github.com/ErhanDgr23/TowerDefenceCaseProject
