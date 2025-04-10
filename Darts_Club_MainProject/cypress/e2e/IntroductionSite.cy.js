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
});
