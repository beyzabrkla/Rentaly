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


## 🖼️ Kullanıcı Arayüzü ve Dinamik Yönetim

Rentaly'nin ön yüzündeki tüm içerikler, güçlü bir Admin Paneli aracılığıyla dinamik olarak yönetilmektedir. İşte uygulamanın öne çıkan bölümleri:

### 1. Ana Sayfa Karşılama ve Hızlı Rezervasyon (Hero Section)
Kullanıcıyı karşılayan ilk ekran, yüksek kaliteli bir görsel ve dinamik araç tipi seçimiyle kurgulanmıştır.
* **Dinamik Yapı:** Arka plan metinleri ve araç kategorileri (Sedan, SUV vb.) veritabanından anlık olarak çekilir.
* **Teknik Detay:** Filtreleme alanları, `RentalIndexViewModel` üzerinden yönetilerek asenkron arama altyapısına hazır hale getirilmiştir.

### 2. Dinamik Araç Filosu (Car Fleet Section)
Kullanıcıların kiralayabileceği araçların listelendiği, şık bir kart tasarımıdır.
* **Admin Kontrolü:** Araçların günlük kira bedeli, yolcu kapasitesi, bagaj hacmi ve vites tipi gibi tüm teknik veriler admin panelinden güncellenmektedir.
* **Tasarım:** Carousel yapısı ile akıcı bir kullanıcı deneyimi sağlanmıştır.

### 3. Kurumsal İstatistikler ve Hakkımızda
Şirket güvenilirliğini artıran sayaç (counter) ve tanıtım bölümüdür.
* **Dinamik Veri:** "Mutlu Müşteri", "Araç Filosu" ve "Deneyim Yılı" gibi rakamlar `ViewComponent` kullanılarak doğrudan veritabanından çekilir; böylece her güncelleme anında yansır.

### 4. Hizmet Özelliklerimiz (Features Section)
Projenin "Neden Biz?" sorusuna cevap verdiği bölümdür.
* **İkonik Yönetim:** FontAwesome ikon sınıfları admin panelinden metin olarak girilir (`fa-solid fa-trophy` vb.) ve arayüzde dinamik olarak render edilir.
* **UI/UX:** Pixel-perfect ikon yerleşimi ve hover efektleri ile görsel bütünlük sağlanmıştır.

### 5. Müşteri Deneyimleri (Testimonials)
Müşterilerin gerçek fotoğrafları ve yorumlarını içeren referans bölümüdür.
* **Mimari:** `TestimonialService` aracılığıyla gelen veriler, frontend tarafında asimetrik bir kart düzeniyle sergilenir.
* **Yönetim:** Admin panelinden yeni yorum eklendiği anda bu kartlar otomatik olarak güncellenir.

### 6. Sıkça Sorulan Sorular (FAQ) ve Footer
Kullanıcıların destek alabileceği son durak noktasıdır.
* **Akordeon Yapısı:** FAQ bölümü tamamen dinamik olup, admin tarafından eklenen her yeni soru-cevap çifti otomatik olarak listede yerini alır.
* **Footer:** İletişim bilgileri, sosyal medya linkleri ve hızlı bağlantılar merkezi bir `layout` üzerinden yönetilir.
<img width="1873" height="952" alt="1" src="https://github.com/user-attachments/assets/a9efe5aa-aec7-4f8f-ac4d-5aeeca9bb59a" />
<img width="1867" height="781" alt="2" src="https://github.com/user-attachments/assets/441e1c75-7366-4b99-aefc-1309f792af40" />
<img width="1868" height="736" alt="3" src="https://github.com/user-attachments/assets/25d69611-7b3e-490c-ba34-0952bdf23b71" />
<img width="1868" height="625" alt="4" src="https://github.com/user-attachments/assets/258ade6a-f356-45ff-a3b5-453173b6ae96" />
<img width="1859" height="740" alt="5" src="https://github.com/user-attachments/assets/795bc72f-d6b6-4cdc-941e-4afb190896a4" />
<img width="1870" height="424" alt="6" src="https://github.com/user-attachments/assets/b1ae1926-6395-4cfd-b514-e679240d16ae" />
<img width="1860" height="944" alt="7" src="https://github.com/user-attachments/assets/1046ee6d-98aa-47c7-bce7-3c9eafa96e3c" />

### 7. Gelişmiş Araç Filtreleme ve Arama Sistemi
Kullanıcıların ihtiyaçlarına en uygun aracı saniyeler içinde bulmasını sağlayan dinamik bir arama motorudur.
* **Teknik Detay:** `LINQ` sorguları ile veritabanı seviyesinde optimize edilmiş filtreleme uygulanmıştır. Seçilen araç tipi, lokasyon ve tarih bilgileri `QueryString` üzerinden Controller'a taşınarak dinamik sonuçlar üretilir.
* **Müsaitlik Kontrolü (Availability Logic):** Sistem, veritabanındaki tüm araçları değil, **yalnızca durumu müsait olan (IsAvailable=true)** ve aktif bir rezervasyonu bulunmayan araçları listeler. Kiralanmış veya bakımda olan araçlar arama sonuçlarında yer almaz.
* **Kategori Bazlı Filtreleme:** Sol menüdeki kategoriler ve koltuk sayısı filtreleri, sayfa yenilenmeden asenkron yapıya uygun şekilde sonuçları daraltma imkanı sunar.
<img width="791" height="575" alt="8 filtrelemeli araç listeleme" src="https://github.com/user-attachments/assets/3914f9d2-6453-4204-ad43-78c3999fdd82" />
<img width="1864" height="997" alt="9" src="https://github.com/user-attachments/assets/f2e1a228-c60d-4517-a3d0-f68f48a5ce40" />

### 8. Kapsamlı Araç Katalogu ve Çoklu Filtreleme
Kullanıcıların geniş araç filosunu detaylı kriterlere göre daraltabildiği merkezi listeleme sayfasıdır.

* **Çok Boyutlu Filtreleme Paneli:** Sol panelde yer alan dinamik filtreler sayesinde kullanıcılar; **Kategori, Koltuk Sayısı, Yakıt Tipi, Şube (Lokasyon)** ve **Marka/Model** bazlı arama yapabilirler.
* **Fiyat Aralığı Filtresi (Range Filter):** Minimum ve maksimum fiyat limitleri girilerek bütçeye uygun araçların anlık listelenmesi sağlanmıştır.
* **Gelişmiş Sayfalama (Pagination):** Performans optimizasyonu amacıyla tüm araçlar tek seferde yüklenmez; `IPagedList` veya benzeri bir sayfalama yapısı ile veriler parçalı olarak sunulur. Bu, büyük veri setlerinde sayfa açılış hızını korur.
* **Müsaitlik ve Durum Yönetimi:** Liste ekranında sadece kiralamaya uygun olan aktif araçlar gösterilir. Her araç kartı; günlük fiyat, vites tipi, kilometre ve lokasyon gibi kritik bilgileri şık bir asimetrik kart tasarımıyla sunar.
<img width="1868" height="950" alt="10" src="https://github.com/user-attachments/assets/e7ae618b-6d05-470a-96a9-0d6a349ff2cf" />
<img width="1857" height="943" alt="11" src="https://github.com/user-attachments/assets/334f6dd7-1345-4a59-a8e0-2c5c95368690" />
<img width="1862" height="511" alt="12" src="https://github.com/user-attachments/assets/b0a652ad-a795-4636-a202-59347cf53a4f" />

### 9. Araç Detay Sayfası ve Dinamik Rezervasyon Sistemi
Kullanıcıların seçtikleri araç hakkında tüm teknik detaylara ulaştığı ve rezervasyon işlemlerini başlattığı kapsamlı modüldür.

* **Zengin İçerik Yönetimi:** Aracın şube bilgisi, plakası, kilometresi, yakıt türü ve vites tipi gibi teknik özellikleri dinamik olarak listelenir. Ayrıca araç hakkında hazırlanan detaylı tanıtım metni ve galeri yapısı ile kullanıcı bilgilendirilir.
* **Otomatik Ücret Hesaplama Algoritması:** Rezervasyon formunda "Alış" ve "Dönüş" tarihleri seçildiği anda, arka planda çalışan bir JavaScript/C# mantığı sayesinde toplam kiralama gün sayısı hesaplanır. 
    * **Formül:** `(Günlük Kira Bedeli * Gün Sayısı) + Depozito Ücreti = Genel Toplam`
    * Bu işlem, kullanıcı henüz sayfayı terk etmeden "Kiralama Özeti" panelinde anlık olarak güncellenir.
* **Hızlı Rezervasyon Formu:** Kullanıcıdan alınan sürücü bilgileri (Ad, Soyad, T.C. Kimlik, Ehliyet Seri No vb.), `FluentValidation` kütüphanesi ile hem istemci (Client-side) hem de sunucu (Server-side) tarafında doğrulanarak veritabanına kaydedilir.
* **Güvenlik ve Validasyon:** T.C. Kimlik No ve Ehliyet Seri No gibi alanlar için özel karakter ve uzunluk kontrolleri uygulanarak veri tutarlılığı sağlanmıştır.
<img width="1868" height="947" alt="13" src="https://github.com/user-attachments/assets/be15943a-ac00-4471-938a-bbf234be847c" />
<img width="1870" height="945" alt="14" src="https://github.com/user-attachments/assets/270b7c58-00e0-476b-bf0a-7b227dd7f591" />
<img width="1871" height="944" alt="15" src="https://github.com/user-attachments/assets/1d32ae98-1f7c-48cc-831e-75c572459228" />

## 🛠️ Gelişmiş Admin Yönetim Paneli

Rentaly'nin tüm operasyonel süreçleri, veriye dayalı kararlar almayı kolaylaştıran kapsamlı bir yönetim paneli üzerinden yürütülmektedir. Admin paneli, sistemdeki tüm dinamik içeriğin merkez üssüdür.

### 1. Sistem Panoraması ve Veri Analitiği (Dashboard)
Yöneticilerin sistemin genel durumunu saniyeler içinde analiz edebilmesi için tasarlanmış özet ekranıdır.

* **Canlı İstatistik Kartları:** Mevcut araç filosu, kayıtlı alt modeller, aktif hizmet noktaları (şubeler) ve segment dağılımı gibi kritik veriler dinamik sayaçlar üzerinden takip edilir.
* **Filo Büyüme Analizi:** `Chart.js` veya benzeri bir grafik kütüphanesi entegrasyonu ile araç sayısındaki aylık artış trendi görselleştirilmiştir. Bu sayede operasyonel büyüme grafik üzerinden izlenebilir.
* **Son Kayıtlar ve Operasyonel Verimlilik:** Sisteme en son dahil edilen araçlar (plaka ve model bazlı) anlık olarak listelenir. Ayrıca, aktif araçların toplam filoya oranını gösteren "Operasyonel Verimlilik" barı ile filo doluluk oranı takip edilir.

### 2. Merkezi Yönetim Modülleri
Panelin sol menüsünde yer alan modüller sayesinde veritabanındaki tüm tablolar üzerinde tam kontrol sağlanır:
* **Araç ve Filo Yönetimi:** Araç listesi, yeni araç ekleme, marka ve model tanımlamaları.
* **Operasyonel Kontrol:** Şube yönetimi, müşteri bilgileri ve kiralama geçmişi takibi.
* **İçerik Yönetimi (CMS):** Banner alanları, "Hakkımızda" metinleri, Sıkça Sorulan Sorular ve Müşteri Yorumları gibi frontend alanlarının dinamik değişimi.
<img width="1875" height="939" alt="16" src="https://github.com/user-attachments/assets/a82bf7b9-a4e5-4757-8ead-c09dfdc42d6c" />

### 3. Gelişmiş Araç ve Filo Yönetimi
Admin paneli üzerinden araç filosunun tüm teknik ve idari verileri üzerinde tam kontrol sağlanmaktadır.

* **Dinamik Listeleme ve Akıllı Filtreleme:** Araç listesi; **Marka, Durum (Müsait/Kirada/Bakımda)** ve **Yakıt Tipi** gibi kriterlere göre anlık olarak filtrelenebilir. Bu özellik, büyük filolarda aranan araca hızlıca ulaşılmasını sağlar.
* **Yeni Araç Ekleme Modülü:** Sisteme yeni bir araç dahil edilirken; araç görselleri, teknik özellikler (vites, yakıt, koltuk sayısı vb.) ve lokasyon bilgileri tek bir form üzerinden kaydedilir.
* **Kapsamlı Düzenleme Sayfası:** Mevcut araçların tüm verileri, `AutoMapper` destekli DTO yapıları sayesinde güvenli bir şekilde güncellenebilir. Araç durumu (Availability) bu panelden manuel olarak da manipüle edilebilir.
* **Görsel ve Teknik Veri Entegrasyonu:** Her araç kartı, sistemde yüklü olan görselleri ve veritabanındaki teknik detayları (plaka, kilometre, depozito ücreti vb.) asenkron olarak çeker ve kullanıcıya sunar.
<img width="1871" height="955" alt="17" src="https://github.com/user-attachments/assets/b5be8593-5c0f-4eec-a02b-6bb58b209cc0" />
<img width="1869" height="946" alt="18" src="https://github.com/user-attachments/assets/fea017ea-b11a-4fcc-8d89-ec8c6ccfb258" />
<img width="1860" height="945" alt="19" src="https://github.com/user-attachments/assets/03bad673-3283-4b7c-b8cb-f572ff912cb4" />
<img width="1872" height="945" alt="20" src="https://github.com/user-attachments/assets/4f858a4d-05b7-413a-ad4e-671e6bd1e43a" />
<img width="1871" height="951" alt="21" src="https://github.com/user-attachments/assets/a52c4847-6738-492d-a37c-6d8feb2ed704" />
<img width="1864" height="950" alt="22" src="https://github.com/user-attachments/assets/64bbdda6-0018-4ad6-890d-962a022099d9" />
<img width="1866" height="945" alt="23" src="https://github.com/user-attachments/assets/6f44260f-4abf-4c07-8e69-fe619441024d" />

### 4. Marka Yönetimi ve İlişkisel Veri Takibi
Sistemdeki araçların bağlı olduğu markaların merkezi olarak yönetildiği modüldür.

* **Durum Bazlı Filtreleme (Aktif/Pasif):** Markalar "Aktif" veya "Pasif" olarak işaretlenebilir. Pasif olan bir markaya ait araçlar, kullanıcı tarafındaki arama sonuçlarında otomatik olarak gizlenir.
* **Markaya Özel Araç Listeleme:** Her markanın yanında bulunan "Araçları Gör" butonu sayesinde, o markaya ait tüm araçlar ilişkisel bir tablo yapısıyla süzülerek getirilir. 
* **Hızlı Güncelleme:** Marka adı, logosu ve durumu üzerinde yapılan değişiklikler, veritabanındaki tüm ilişkili araçları anlık olarak etkiler.
<img width="1869" height="948" alt="24" src="https://github.com/user-attachments/assets/65714a2b-5f24-4ca3-8791-f1e4787038cd" />

### 5. Araç Model Yönetimi ve Hiyerarşik Yapı
Marka altındaki model tanımlamalarının yapıldığı ve yönetildiği, hiyerarşik veri yapısını tamamlayan modüldür.

* **Aktif/Pasif Durum Kontrolü:** Modeller bazında listeleme yapılarak, operasyonel duruma göre modeller aktif veya pasif hale getirilebilir.
* **Model Bazlı Araç Süzme:** Spesifik bir modele (Örn: Tesla Model Y) ait tüm araçlar tek bir tıkla anlık olarak listelenebilir.
* **Yeni Model Tanımlama:** Seçilen bir markaya bağlı olarak yeni model isimleri sisteme dahil edilebilir; bu yapı veritabanında `MarkaID` üzerinden kurulan güçlü bir ilişkisel tablo (One-to-Many) ile yönetilir.
* **Kullanıcı Dostu UI:** Model listesi üzerinde markaların logoları ve isimleri bir arada sunularak görsel yönetim kolaylaştırılmıştır.
<img width="1867" height="931" alt="25" src="https://github.com/user-attachments/assets/5fce9268-a80c-4031-9d4c-5a1a4188ee46" />
<img width="1859" height="941" alt="25-1" src="https://github.com/user-attachments/assets/9c27fe4f-3d4a-491b-a334-66204f566491" />
<img width="480" height="365" alt="25-2" src="https://github.com/user-attachments/assets/6aecc1f0-f131-4d72-a03c-ecd05725c41e" />
<img width="431" height="455" alt="25-3" src="https://github.com/user-attachments/assets/67956c3c-d891-412c-ac5f-a7529f5d86ae" />

### 6. Araç Model Yönetimi ve Hiyerarşik Yapı
Marka altındaki model tanımlamalarının yapıldığı ve yönetildiği, projenin hiyerarşik veri yapısını (Marka -> Model -> Araç) tamamlayan modüldür.

* **Aktif/Pasif Durum Kontrolü:** Modeller bazında listeleme yapılarak, operasyonel duruma göre modeller aktif veya pasif hale getirilebilir.
* **Model Bazlı Araç Süzme:** Spesifik bir modele (Örn: Tesla Model Y) ait tüm araçlar tek bir tıkla anlık olarak listelenebilir.
* **Yeni Model Tanımlama ve Güncelleme:** Seçilen bir markaya bağlı olarak yeni modeller sisteme dahil edilebilir veya mevcut modellerin marka ilişkileri güncellenebilir. Bu yapı, veritabanında `MarkaID` üzerinden kurulan güçlü bir ilişkisel tablo (One-to-Many) ile yönetilir.
* **Kullanıcı Dostu UI:** Model listesi üzerinde markaların logoları ve isimleri bir arada sunularak görsel yönetim kolaylaştırılmıştır.
<img width="1869" height="952" alt="26" src="https://github.com/user-attachments/assets/18510383-e704-4f62-870f-f021f9dfb048" />
<img width="496" height="559" alt="26-1" src="https://github.com/user-attachments/assets/3e407597-87fc-420e-aaab-46282cbd4669" />
<img width="536" height="431" alt="26-" src="https://github.com/user-attachments/assets/a4c653ac-82c3-4d84-b00b-03dec9484016" />


### 7. Kategori Yönetimi ve Dinamik Segmentasyon
Araçların segmentlerine (Sedan, SUV, Minivan vb.) göre ayrıştırıldığı ve ön yüzdeki filtreleme sistemini besleyen temel yönetim modülüdür.

* **Durum Kontrolü (Aktif/Pasif):** Kategoriler sistem üzerinden aktif veya pasif hale getirilebilir. Pasif edilen bir kategori, kullanıcı arayüzündeki arama ve filtreleme seçeneklerinden otomatik olarak kaldırılır.
* **Kategori Bazlı Araç Analizi:** Her kategorinin yanında bulunan "Araçları Gör" butonu ile o segmente ait tüm araçlar ilişkisel veri tabanı sorguları (LINQ) aracılığıyla listelenir.
* **Dinamik İçerik Yönetimi:** Yeni kategori ekleme ve mevcut kategorileri güncelleme işlemleri; `Entity Framework Core` ve `Generic Repository` desenleri kullanılarak güvenli bir şekilde gerçekleştirilir.
* **İlişkisel Veri Bütünlüğü:** Kategoriler üzerinde yapılan değişiklikler, bu kategoriye bağlı tüm araç tanımlamalarıyla eşzamanlı olarak çalışır.
<img width="1871" height="942" alt="Ekran görüntüsü 2026-04-09 172009" src="https://github.com/user-attachments/assets/5d7f07eb-6d27-45ac-9f3a-c4a489fb4196" />
<img width="504" height="288" alt="Ekran görüntüsü 2026-04-09 172017" src="https://github.com/user-attachments/assets/54df4bc4-5c01-4f6f-9f5e-2c3329f50794" />
<img width="459" height="271" alt="Ekran görüntüsü 2026-04-09 172022" src="https://github.com/user-attachments/assets/102f22b4-e991-451e-a580-7f3be0b0427c" />

### 8. Şube ve Lokasyon Yönetimi
Uygulamanın hizmet noktalarının merkezi olarak yönetildiği ve araçların fiziksel konumlarına göre gruplandırıldığı modüldür.

* **Dinamik Şube Listesi:** Tüm aktif ve pasif şubeler, lokasyon bilgileriyle birlikte listelenir. Şube durumu (Aktif/Pasif) üzerinden yapılan değişiklikler, kullanıcı tarafındaki lokasyon seçimlerini doğrudan günceller.
* **Şube Bazlı Araç Gruplandırma:** Her şubenin yanında bulunan "Araçları Gör" butonu ile o lokasyonda (Örn: İstanbul Merkez, Ankara Şube) bulunan tüm araçlar süzülerek listelenir. Bu özellik, saha operasyonlarının takibini kolaylaştırır.
* **Yeni Lokasyon Tanımlama:** Genişleyen hizmet ağına paralel olarak sisteme yeni şubeler eklenebilir. Şube ekleme sürecinde `EntityLayer` üzerindeki `Branch` varlığı kullanılarak koordineli bir veri girişi sağlanır.
* **Şube Güncelleme Paneli:** Mevcut şubelerin iletişim, adres ve durum bilgileri `AutoMapper` entegrasyonu ile güvenli bir şekilde güncellenerek veritabanı tutarlılığı korunur.
<img width="1874" height="952" alt="27" src="https://github.com/user-attachments/assets/e37a8ef1-06eb-4a8d-90a4-e641b5a2e38e" />
<img width="1863" height="943" alt="28" src="https://github.com/user-attachments/assets/3ca12fdf-5edb-4667-9d0d-628084cc78f0" />
<img width="558" height="704" alt="29" src="https://github.com/user-attachments/assets/b8a993b7-a5b4-4747-b5e9-98ec8b70cab7" />

### 7. Rezervasyon ve Müşteri İlişkileri Yönetimi (CRM)
Kiralama süreçlerinin uçtan uca takip edildiği, onay mekanizmalarının ve müşteri bilgilendirme sistemlerinin bulunduğu modüldür.

* **Onaylı Müşteri Listesi:** Rezervasyon süreci başarıyla tamamlanan ve onaylanan müşteriler, merkezi bir listede takip edilir. Bu liste, aktif kiralama yapan kullanıcıların verilerine hızlı erişim sağlar.
* **Kapsamlı Rezervasyon Takibi:** Sistemdeki tüm rezervasyonlar (Beklemede, Onaylandı, İptal Edildi vb.) tek bir panel üzerinden yönetilir. Durum bazlı filtreleme ile operasyonel yoğunluk kolayca analiz edilebilir.
* **Detaylı Rezervasyon Modalı:** Her bir rezervasyon için özel olarak tasarlanmış modal yapısı sayesinde, sayfadan ayrılmadan; kiralama tarihleri, araç detayları ve sürücü bilgilerine ait tüm ayrıntılar görüntülenebilir.
* **Otomatik E-Posta Bildirim Sistemi:** Rezervasyon onaylandığı anda müşteriye otomatik olarak bir konfirmasyon maili gönderilir. Bu entegrasyon, kullanıcı güvenini artırırken operasyonel iş yükünü minimize eder.
<img width="1865" height="944" alt="30" src="https://github.com/user-attachments/assets/3a13c67c-214c-40e9-873b-52b7cfc55bd6" />
<img width="1873" height="949" alt="31" src="https://github.com/user-attachments/assets/3f899f28-8d9d-41b6-aea0-e27f843c7fc6" />
<img width="658" height="905" alt="32" src="https://github.com/user-attachments/assets/3a165cd8-5620-4b57-89f8-ad3f59c402da" />
![WhatsApp Image 2026-04-09 at 17 25 01](https://github.com/user-attachments/assets/800c9b71-37b0-4d04-9029-12e973ddebde)



