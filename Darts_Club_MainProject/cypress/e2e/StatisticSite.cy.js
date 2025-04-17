/// <reference types="Cypress"/>

describe("Statisztika oldal tesztelése", () => {
    beforeEach(() => {
        cy.loginProfile('jancsika@gmail.com', 'jancsika');
        cy.get('[data-cy="statistic_box"]').click();
        cy.scrollTo('top');
    }) 

    it('Az elnavigálás megtörént', () => {
        cy.url().should('contain', 'http://localhost:5173/statistic');
    })

    it('Felhasználó adatainak ellenőrzése', () => {
        cy.get('[data-cy="username_title"]').should('have.text', 'Jancsika');
        cy.get('[data-cy="numof_matches"]').should('contain', '10');
        cy.get('[data-cy="win_matches"]').should('contain', '4');
        cy.get('[data-cy="register_date"]').should('contain', '04/12/2025');
        cy.get('[data-cy="user_level"]').should('have.text', 'Amatőr');
        cy.get('[data-cy="user_level"]').should('have.css', 'color', 'rgb(25, 135, 84)');
        cy.get('[data-cy="level_bar"]').should('have.css', 'background-color', 'rgb(25, 135, 84)');
    })

    it('Módosítás gomb megjelenik a felhasználónak', () =>{
        cy.get('[data-cy="modify_btn"]').should('be.visible');
    })

    
})