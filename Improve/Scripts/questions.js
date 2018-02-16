'use strict';

/* DOM */

var dom = function () {
    return {
        id: (id) => document.getElementById(id)
        , class: (cl) => document.getElementsByClassName(cl)[0]
        , classes: (cls) => document.getElementsByClassName(cls)
        , q: (selector) => document.querySelector(selector)
        , create: (tag) => document.createElement(tag)
        , add: (parent, child) => parent.appendChild(child)
    }
}();

/* Questions */

var btnStart = dom.class('try');
btnStart.addEventListener('click', start);

function start() {
    var textAboveButton = dom.id('txttt');
    textAboveButton.classList.add('disappear-txttt');

    var blankForm = dom.class('blnk');
    blankForm.classList.add('disappear-blank');

    var fourthSection = dom.class('section-four');
    fourthSection.classList.add('recolor-section');

    setTimeout(function () {
        var startt = dom.id('txttt');
        startt.style.display = "none";
        var bg = dom.class('section-four');
        bg.style.background = "#012535";
        var ques = dom.id('questions');
        ques.style.display = "block";
    }, 300)
}

var ans = document.getElementsByClassName('ans');
for (var i = 0; i < ans.length; i++) {
    ans[i].addEventListener('click', slide);
}

function slide() {
    var prev = dom.class('prev');
    prev.classList.add('disappear-prev');

    var curr = dom.class('current');
    curr.classList.add('slide-up');

    for (let i = 0; i <= curr.children.length; i += 2) {
        curr.children[i].classList.add('recolor-prev');
    }

    setTimeout(function () {
        var prev = dom.class('prev');
        prev.parentNode.removeChild(prev);// prev del

        var newClass = dom.class('new');
        newClass.className = "current";// new del / curr add
        newClass.classList.add('appear-new');
        //newClass.classList.add('slide-up');


        var curr = dom.class('current');


        var newDiv = dom.create('div');
        newDiv.className = 'new';
        curr.parentNode.appendChild(newDiv);// new add

        curr.className = "prev";// curr del / prev add
        curr.classList.add('slide-current');
        for (let i = 0; i <= curr.children.length; i += 2) {
            if (i != 0) {
                curr.children[i].classList.add('transform-width');
            }
        }
    }, 400)
}