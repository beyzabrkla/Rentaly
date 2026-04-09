# 🚗 Rentaly - Modern Araç Kiralama Portalı
**Rentaly**, .NET 8.0 ve gelişmiş yazılım mimarileri kullanılarak inşa edilmiş, ölçeklenebilir ve yönetilebilir bir araç kiralama platformudur. Proje, kurumsal tasarım desenleri (Design Patterns) üzerine kurgulanmış olup, hem kullanıcı hem de admin tarafında tam otomasyon sağlamaktadır.

## 🚀 Teknolojiler ve Mimari Altyapı
Proje, sürdürülebilir kod prensipleri ve kurumsal standartlara uygun olarak aşağıdaki teknolojilerle inşa edilmiştir:

### 🛠️ Teknik Yığın (Tech Stack)
* **Framework:** .NET 8.0 (ASP.NET Core MVC)
* **Veritabanı:** MSSQL Server & Entity Framework Core (Code First)
* **Validasyon:** FluentValidation (Business ve UI katmanlı kontrol)
* **Mapping:** AutoMapper (Entity-DTO dönüşümleri için)
* **Raporlama:** ClosedXML (Excel) & DinkToPdf (PDF Raporlama)
* **Frontend:** Bootstrap 5, Tailwind CSS, FontAwesome 6, ViewComponents yapısı.

### 🏗️ Kurumsal Mimari Yapısı (N-Tier Architecture)
Proje 5 ana katman üzerine inşa edilmiştir:
1.  **Rentaly.WebUI:** Uygulamanın sunum katmanıdır. **Areas (Admin)** yapısı, **ViewComponents** kullanımı ve asenkron Controller yapıları ile optimize edilmiştir.
2.  **Rentaly.BusinessLayer:** İş mantığının (Business Logic) yürütüldüğü katmandır. `Abstract (Services)` ve `Concrete (Managers)` yapısı ile servis odaklı çalışır.
3.  **Rentaly.DataAccessLayer:** Veri erişim katmanıdır. **Unit of Work** ve **Generic Repository** desenleri kullanılarak veritabanı bağımsızlığı ve işlem güvenliği sağlanmıştır.
4.  **Rentaly.DTOLayer:** Veri transfer nesnelerini (DTO) barındırır. `Create`, `Result`, `Update` bazlı ayrıştırılmış DTO yapıları ile veri güvenliği ve performans optimize edilmiştir.
5.  **Rentaly.EntityLayer:** Veritabanı tablolarına karşılık gelen temel sınıfları (Entities) barındırır.

## 💎 Uygulanan Design Patterns & Prensipler
* **Unit of Work:** Veritabanı işlemlerinin toplu ve tutarlı bir şekilde yönetilmesini sağlar.
* **Generic Repository Pattern:** Kod tekrarını önleyerek merkezi bir veri erişim noktası sunar.
* **Dependency Injection (DI):** Nesne bağımlılıklarının gevşek bağlı (loosely coupled) yönetimini sağlar.
* **DTO (Data Transfer Object):** Hassas verilerin UI katmanına sızmasını önler.
* **Service Oriented Architecture:** İş mantığını bağımsız servisler üzerinden yürütür.

## 🛠️ Uygulama Özellikleri

### 👨‍💻 Gelişmiş Admin Yönetimi
* **Dinamik İşleyiş (Process):** Adım adım kiralama süreçlerinin ikon ve içerik yönetimi.
* **Referanslar (Testimonials):** Müşteri yorumlarının CRUD ve görsel yönetimi.
* **SSS (FAQ):** Soru-cevap bankasının dinamik olarak güncellenmesi.
* **Raporlama:** Excel (ClosedXML) ve PDF (DinkToPdf) formatlarında veri dökümü alma.

### 🎨 UI/UX Tasarımı
* **Pixel-Perfect UI:** Photoshop ile optimize edilmiş görsel yerleşimler.
* **Responsive Tasarım:** Mobil, tablet ve masaüstü cihazlarla tam uyumluluk.

