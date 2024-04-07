db.collections.drop('usuaris');
db.collections.drop('packs');
db.collections.drop('clients');
db.collections.drop('reparacions');
db.collections.drop('factures');

// Insertem usuaris
db.usuaris.insertMany([
  {
    login: "admin",
    password: "admin",
    tipus: "admin",
  },
  {
    login: "user1",
    password: "user1",
    tipus: "mecanic",
  },
  {
    login: "user2",
    password: "user2",
    tipus: "recepcio",
  },
]);

// Insertem un pack
db.packs.insertMany([
  {
    descripcio: "Revisió general",
    preu: 200,
  }
]);

// Insertem 10 clients
db.clients.insertMany([
  {
    nif: "12345678A",
    nom: "Bonifacio",
    cognoms: "Garcia López",
    telefon: "600123456",
    adreça: "Calle Falsa, 123",
    vehicles: [
      {
        matricula: "1234ABC",
        km: 20000,
        model:
          "Ford Mustang (03.2023) Fastback 5.0 Ti-VCT V8 460CV Aut. Fastback Mach 1",
      },
      {
        matricula: "5678DEF",
        km: 10000,
        model: "Toyota GR SUPRA Performance/Performance special edition(MY22)",
      },
    ],
  },
  {
    nif: "23456789B",
    nom: "Eulogio",
    cognoms: "Fernández Ruiz",
    telefon: "601234567",
    adreça: "Avenida de la Estación, 45",
    vehicles: [
      {
        matricula: "91011GHI",
        km: 5000,
        model:
          "Renault TWIZY E-TECH 100% eléctrico L6e ( Twizy45 ) (Semi: URB 3 45 FP / URB 3 45)",
      },
    ],
  },
  {
    nif: "34567890C",
    nom: "Prudencio",
    cognoms: "Martinez Gómez",
    telefon: "602345678",
    adreça: "Plaza Mayor, 1",
    vehicles: [
      {
        matricula: "121314JKL",
        km: 15000,
        model: "Toyota PRIUS PHEV Advance / Luxury (MY21)",
      },
      {
        matricula: "151617MNO",
        km: 22000,
        model: "Toyota Canarias COROLLA HB Híbrido 1.8 Agua",
      },
    ],
  },
  {
    nif: "45678901D",
    nom: "Teófilo",
    cognoms: "Sánchez Pérez",
    telefon: "603456789",
    adreça: "Calle Ancha, 56",
    vehicles: [
      {
        matricula: "181920PQR",
        km: 30000,
        model:
          "Volkswagen Turismos Golf 8 MY21 Golf Life 1.5 eTSI 96 kW (130 CV) Automatico DSG 7 vel.",
      },
    ],
  },
  {
    nif: "56789012E",
    nom: "Fulgencio",
    cognoms: "Jiménez Rodriguez",
    telefon: "604567890",
    adreça: "Calle del Pozo, 7",
    vehicles: [
      {
        matricula: "212223STU",
        km: 12000,
        model: "Opel Corsa (MY19.0) 5p DESIGN LINE 1.4 GLP 90 CV (MY19.0)",
      },
      {
        matricula: "242526VWX",
        km: 8000,
        model:
          "Opel Astra 5P (MY19.5) 120 ANIVERSARIO 1,0 Turbo S/S 105 cv 5V MY",
      },
    ],
  },
  {
    nif: "67890123F",
    nom: "Jacinto",
    cognoms: "López Martin",
    telefon: "605678901",
    adreça: "Avenida del Parque, 34",
    vehicles: [
      {
        matricula: "272829YZA",
        km: 24000,
        model:
          "Nissan Nuevo Qashqai DIG-T E6D 103 KW (140CV) Mild Hybrid 12V MT ACENTA",
      },
    ],
  },
  {
    nif: "78901234G",
    nom: "Rodrigo",
    cognoms: "González Navarro",
    telefon: "606789012",
    adreça: "Paseo de la Castellana, 120",
    vehicles: [
      {
        matricula: "303132BCD",
        km: 18000,
        model:
          "Hyundai TUCSON                  1.6 CRDI 136CV 48V 4X4 DT STYLE",
      },
      {
        matricula: "333435EFG",
        km: 20000,
        model: "Hyundai i30 5P CRDI 1.6 116CV KLASS LR",
      },
    ],
  },
  {
    nif: "89012345H",
    nom: "Gregorio",
    cognoms: "Diaz Castro",
    telefon: "607890123",
    adreça: "Calle Nueva, 88",
    vehicles: [
      {
        matricula: "363738HIJ",
        km: 11000,
        model: "SEAT Arona 1.0 TGI 66KW XCELLENCE MY19",
      },
    ],
  },
  {
    nif: "90123456I",
    nom: "Hermenegildo",
    cognoms: "Torres Pardo",
    telefon: "608901234",
    adreça: "Ronda Sur, 5",
    vehicles: [
      {
        matricula: "394041KLM",
        km: 9500,
        model:
          "SKODA KODIAQ MY23 SPORTLINE 2.0 TDI 110 kW (150 CV) AdBlue DSG 4x2 (MTMA 2431)",
      },
      {
        matricula: "424344NOP",
        km: 7500,
        model:
          "Land Rover Range Rover Sport 3.0D SDV6 306CV 4WD Auto - SE/HSE/HSE Dynamic/Autobiography Dynamic - 5 asientos",
      },
    ],
  },
  {
    nif: "01234567J",
    nom: "Aniceto",
    cognoms: "Ruiz López",
    telefon: "609012345",
    adreça: "Avenida de América, 32",
    vehicles: [
      {
        matricula: "454647QRS",
        km: 6500,
        model: "Lexus RX 450h 5 plazas Llanta 20p (MY20)",
      },
    ],
  },
]);

// Insertem reparacions
db.reparacions.insertMany([
  {
    estat: "oberta",
    data: new Date("2024-02-10"),
    vehicle_id: "1234ABC",
    linies: [
      {
        numero: "1",
        descripció: "Canvi d'oli i filtre",
        tipus: "feina",
        quantitat: 1,
      },
      {
        numero: "2",
        descripció: "Filtre de l'aire",
        tipus: "peça",
        codi_fabricant: "FA123",
        preu_unitat: 25,
        quantitat: 2,
      },
      {
        numero: "3",
        descripció: "Revisió de frens",
        tipus: "feina",
        quantitat: 2,
      },
    ],
  },
  {
    estat: "facturada",
    data: new Date("2024-02-05"),
    vehicle_id: "5678DEF",
    linies: [
      {
        numero: "1",
        descripció: "Canvi de rodes",
        tipus: "peça",
        codi_fabricant: "NE456",
        preu_unitat: 100,
        quantitat: 4,
        preu: 400,
      },
      {
        numero: "2",
        descripció: "Alineat de direcció",
        tipus: "feina",
        descompte: 20,
        quantitat: 2,
        preu: 80,
      },
    ],
    factura: 1,
  },
  {
    estat: "oberta",
    data: new Date("2024-02-15"),
    vehicle_id: "121314JKL",
    linies: [
      {
        numero: "1",
        descripció: "Revisió general",
        tipus: "pack",
        preu: 200,
      },
      {
        numero: "2",
        descripció: "Canvi de bateria",
        tipus: "peça",
        codi_fabricant: "BA789",
        preu_unitat: 120,
        quantitat: 1,
      },
      {
        numero: "3",
        descripció: "Diagnòstic electrònic",
        tipus: "feina",
        quantitat: 1,
      },
    ],
  },
  {
    estat: "facturada",
    data: new Date("2024-02-08"),
    vehicle_id: "394041KLM",
    linies: [
      {
        numero: "1",
        descripció: "Canvi de corretja de distribució",
        tipus: "feina",
        quantitat: 4,
        preu: 240,
      },
      {
        numero: "2",
        descripció: "Kit d'embragatge",
        tipus: "peça",
        codi_fabricant: "EM012",
        preu_unitat: 350,
        quantitat: 2,
        preu: 700,
      },
    ],
    factura: 2,
  },
]);

const reparacio1 = db.reparacions.findOne({ factura: 1})._id;
const reparacio2 = db.reparacions.findOne({ factura: 2})._id;

// Insertem factures
db.factures.insertMany([
  {
    numero: 1,
    estat: "pagada",
    data: new Date("2024-02-05"),
    tipus_IVA: "21%",
    preu_ma_obra: 50,
    reparacio_id: reparacio1,
    subtotal: 480,
    import_IVA: 100.8,
    total: 580.8,
  },
  {
    numero: 2,
    estat: "pendent",
    data: new Date("2024-02-08"),
    tipus_IVA: "21%",
    preu_ma_obra: 60,
    reparacio_id: reparacio2,
    subtotal: 1000,
    import_IVA: 210,
    total: 1210,
  },
]);
