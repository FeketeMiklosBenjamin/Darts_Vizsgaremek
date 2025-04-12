/// <reference types="Cypress"/>

describe("Regisztrációs oldal tesztelése", () => {
    beforeEach(() => {
        cy.visit('/registration');
    })

    it('Nem egyező jelszók megadása', () => {
        cy.get('[data-cy="username_input"]').type('franciska');
        cy.get('[data-cy="email_input"]').type('franciska@gmail.com');
        cy.get('[data-cy="password_input"]').type('igeins');
        cy.get('[data-cy="SecondPassword_input"]').type('igenis');
        cy.get('[data-cy="registration_btn"]').click();
        cy.get('[data-cy="error_message"]').should('have.text', 'A két jelszó nem egyezik meg!');
        cy.scrollTo('top');
    })

    it('Felhasználónév üresen hagyása', ()=> {
        cy.get('[data-cy="email_input"]').type('asdasd@gmail.com');
        cy.get('[data-cy="password_input"]').type('igenis');
        cy.get('[data-cy="SecondPassword_input"]').type('igenis');
        cy.get('[data-cy="registration_btn"]').click();
        cy.get('[data-cy="error_message"]').should('have.text', 'A felhasználónév nem lehet üres.');
        cy.scrollTo('top');
    })

    it('Jelszó nem elég hosszú', ()=> {
        cy.get('[data-cy="username_input"]').type('franciska');
        cy.get('[data-cy="email_input"]').type('asdasd@gmail.com');
        cy.get('[data-cy="password_input"]').type('ig');
        cy.get('[data-cy="SecondPassword_input"]').type('ig');
        cy.get('[data-cy="registration_btn"]').click();
        cy.get('[data-cy="error_message"]').should('have.text', 'A jelszónak legalább 8 karakter hosszúnak kell lennie.');
        cy.scrollTo('top');
    })

    it('Sikeresen létrehozva új felhasználó', ()=> {
        cy.intercept('POST', 'http://localhost:5181/api/users/register').as('reg');
        cy.get('[data-cy="username_input"]').type('Józsika');
        cy.get('[data-cy="email_input"]').type('jozsika@freemail.com');
        cy.get('[data-cy="password_input"]').type('jozsikajozsika');
        cy.get('[data-cy="SecondPassword_input"]').type('jozsikajozsika');
        cy.get('[data-cy="registration_btn"]').click();
        cy.wait('@reg');
        cy.url().should('eq', 'http://localhost:5173/main-page');
        cy.scrollTo('top');
    })
})