/// <reference types="Cypress"/>

describe("Kezdő oldal tesztelése", () => {
  beforeEach(() => {
    cy.visit('/');
  })

  it("Regisztrációs gomb elnavigálása megtörténik",() => {
    cy.get('[data-cy="registry_btn"]').click();
    cy.scrollTo('top');
    cy.url().should('eq', 'http://localhost:5173/registration');
  })

  it("Bejelentkezés gomb elnavigálása megtörténik",() => {
    cy.get('[data-cy="login_btn"]').click();
    cy.scrollTo('top');
    cy.url().should('eq', 'http://localhost:5173/sign-in');
  })

  it("Bejelentkezés feliratra kattintással navigálás",() => {
    cy.get('[data-cy="registry_btn"]').click();
    cy.scrollTo('top');
    cy.url().should('eq', 'http://localhost:5173/registration');
  })
//   let LoginBtn, passwordInput, emailInput;
  
//   beforeEach(() => {
//     cy.visit('/sign-in')
//     LoginBtn = cy.get('[data-cy="login_btn"]');
//     emailInput = cy.get('[data-cy="email_input"]');
//     passwordInput = cy.get('[data-cy="password_input"]');
//   });

//   it("1.Teszt: Nem megfelelő adatok ", () => {
//     emailInput.type("Sziahelohelo@gmail.com");
//     passwordInput.type("Sziaszztokhalohalo");
//     LoginBtn.click();
//     cy.get('[data-cy="alert_message"]').should(
//       "have.text",
//       "Hibás email cím vagy jelszó."
//     );
//   });
//   it("2. Teszt: Sikeres bejelentkezés",()=>{
//     emailInput.type("jancsika@gmail.com");
//     passwordInput.type("jancsika");
//     LoginBtn.click();
//     cy.url().should('eq', 'http://localhost:5173/main-page');
//   });
//   it("3.Teszt: Sikeres bejelentkezést követően megjelenik a felhasználó neve",()=>{
//     emailInput.type("jancsika@gmail.com");
//     passwordInput.type("jancsika");
//     LoginBtn.click();
//     cy.url().should('eq', 'http://localhost:5173/main-page');
//     cy.get('.nav-link').should("have.text", "Jancsika")
//   });


// describe("Registration", ()=>{
//     let registryBtn;
//     beforeEach(() => {
//       cy.visit("/registration");
//       registryBtn = cy.get('[data-cy="regBtn"]');
      
//     });
//     it("Teszt 4: ", ()=>{
//       registryBtn.click();
//       cy.get('[data-cy="alert_message"]').should('have.text', 'A két jelszó nem egyezik meg!');
//     })
  // })
});
