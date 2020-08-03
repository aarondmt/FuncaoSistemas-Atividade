var botoesAlterarExcluirBeneficiario = '<button class="btn btn-info btn-sm" name="BtnAlterarBeneficiario" onclick="AlterarBeneficiario(this);" type="button">Alterar</button>' +
    '&nbsp;&nbsp;' +
    '<button class="btn btn-info btn-sm" name="BtnExcluirBeneficiario" onclick="ExcluirBeneficiario(this);" type="button">Excluir</button>';

$(document).ready(function () {
    $('.cpf').mask('999.999.999-99');

    $('.cpf').focusout(function () {
        if ($(this).val().length == 11) {
            $(this).unmask();
            $(this).mask('999.999.999-99');
        }
    });

    $('#BtnBeneficiario').click(function (e) {
        e.preventDefault();
        $('#GridBeneficiario tbody tr').each(function (i, v) {
            if ($(v).attr('data-alterado') == 'true') {
                $(v).find('button[name="BtnCancelarAlateracao"]').click();
            }
        });
        $('#ModalBeneficiario').modal('show');
    });

    $('#AdicionaBeneficiario').click(function (e) {
        e.preventDefault();
        var cpf = $('#BeneficiarioCpf').val();
        var nome = $('#BeneficiarioNome').val();
        if (validaCpf(cpf)) {
            if (VerificaExisteCpfNaTabela(cpf)) {
                ModalDialog("Ocorreu um erro", "O CPF informado já esta na tabela.");
            } else {
                var linha = '<tr data-alterado="false">' +
                    '<td>' + cpf + '</td>' +
                    '<td>' + nome + '</td>' +
                    '<td>' +
                    botoesAlterarExcluirBeneficiario +
                    '</td>' +
                    '</tr>';
                $('#GridBeneficiario tbody').append(linha);
                $("#formBeneficiario")[0].reset();
            }
        }
    });

    $('#formCadastro').submit(function (e) {
        e.preventDefault();
        var cpf = $(this).find("#Cpf").val();
        
        if (validaCpf(cpf)) {
            var arrBeneficiario = [];
            $('#GridBeneficiario tbody tr').each(function (i, v) {
                var beneficiario = {
                    CPF: $(v).find('td:first-child').text(),
                    Nome: $(v).find('td:nth-child(2)').text()
                };
                arrBeneficiario.push(beneficiario)
            });
            
            $.ajax({
                url: urlPost,
                method: "POST",
                data: {
                    "NOME": $(this).find("#Nome").val(),
                    "CEP": $(this).find("#CEP").val(),
                    "Email": $(this).find("#Email").val(),
                    "Sobrenome": $(this).find("#Sobrenome").val(),
                    "Nacionalidade": $(this).find("#Nacionalidade").val(),
                    "Estado": $(this).find("#Estado").val(),
                    "Cidade": $(this).find("#Cidade").val(),
                    "Logradouro": $(this).find("#Logradouro").val(),
                    "Telefone": $(this).find("#Telefone").val(),
                    "Cpf": $(this).find("#Cpf").val(),
                    "ListaBeneficiario": arrBeneficiario
                },
                error:
                    function (r) {
                        if (r.status == 400)
                            ModalDialog("Ocorreu um erro", r.responseJSON);
                        else if (r.status == 500)
                            ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                    },
                success:
                    function (r) {
                        ModalDialog("Sucesso!", r)
                        $("#formCadastro")[0].reset();
                        $('#GridBeneficiario tbody').html('');
                        $("#formBeneficiario")[0].reset();
                    }
            });
        } else {
            ModalDialog('Atenção!', 'O CPF informado não é um CPF válido!')
        }
    })
    
})

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}

function AlterarBeneficiario(element) {
    var linhaParaAlterar = $(element).parents('tr');
    var tdCpf = $(linhaParaAlterar).find('td:first-child');
    var tdNome = $(linhaParaAlterar).find('td:nth-child(2)');
    var tdBotoes = $(linhaParaAlterar).find('td:nth-child(3)');
    $(linhaParaAlterar).attr('data-alterado', true);

    var cpf = $(tdCpf).text();
    var nome = $(tdNome).text();

    var botoes = '<button class="btn btn-success btn-sm" onclick="SalvarAlteracaoBeneficiario(this);" type="button">Salvar</button>' +
        '&nbsp;&nbsp;' +
        '<button class="btn btn-danger btn-sm" name="BtnCancelarAlateracao" onclick="CancelarAlteracaoBeneficiario(this, \'' + cpf + '\', \'' + nome + '\');" type="button">Cancelar</button>';

    $(tdCpf).html('<input type="text" class="form-control cpf" value="' + cpf + '" placeholder="Ex.: 000.000.000-00" />');
    $(tdNome).html('<input type="text" class="form-control" value="' + nome + '" placeholder="Ex.: João da Silva" />');
    $(tdBotoes).html(botoes)
    $('.cpf').mask('999.999.999-99');
    $('.cpf').focusout(function () {
        if ($(this).val().length == 11) {
            $(this).unmask();
            $(this).mask('999.999.999-99');
        }
    });
}

function SalvarAlteracaoBeneficiario(element) {
    var linhaParaAlterar = $(element).parents('tr');
    var tdCpf = $(linhaParaAlterar).find('td:first-child');
    var tdNome = $(linhaParaAlterar).find('td:nth-child(2)');
    var tdBotoes = $(linhaParaAlterar).find('td:nth-child(3)');

    var cpf = $(tdCpf).find('input').val();
    var nome = $(tdNome).find('input').val();

    if (validaCpf(cpf)) {
        if (VerificaExisteCpfNaTabela(cpf)) {
            ModalDialog("Ocorreu um erro", "O CPF informado já esta na tabela.");
        } else {
            $.ajax({
                url: urlPostBeneficiario,
                method: "POST",
                data: {
                    "NOME": nome,
                    "CPF": cpf
                },
                error:
                    function (r) {
                        if (r.status == 400)
                            ModalDialog("Ocorreu um erro", r.responseJSON);
                        else if (r.status == 500)
                            ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                    },
                success:
                    function (r) {
                        $(linhaParaAlterar).attr('data-alterado', false);
                        $(tdCpf).html(cpf);
                        $(tdNome).html(nome);
                        $(tdBotoes).html(botoesAlterarExcluirBeneficiario);
                    }
            });
        }
    }    
}

function CancelarAlteracaoBeneficiario(element, cpfAntigo, nomeAntigo) {
    var linhaParaAlterar = $(element).parents('tr');
    var tdCpf = $(linhaParaAlterar).find('td:first-child');
    var tdNome = $(linhaParaAlterar).find('td:nth-child(2)');
    var tdBotoes = $(linhaParaAlterar).find('td:nth-child(3)');

    $(linhaParaAlterar).attr('data-alterado', false);

    $(tdCpf).html(cpfAntigo);
    $(tdNome).html(nomeAntigo);
    $(tdBotoes).html(botoesAlterarExcluirBeneficiario);

}

function ExcluirBeneficiario(element) {
    $(element).parents('tr').remove();
}

function VerificaExisteCpfNaTabela(cpf) {
    var encontrou = false;
    $('#GridBeneficiario tbody tr').each(function (i, v) {
        var cpfTable = $(v).find('td:first-child').text();
        if (cpfTable == cpf) {
            encontrou = true;
            return;
        }
    });

    return encontrou;
}

function validaCpf(val) {
    
    if (val.length == 14) {
        var cpf = val.trim();
        
        cpf = cpf.replace(/\./g, '');
        cpf = cpf.replace('-', '');
        cpf = cpf.split('');

        var v1 = 0;
        var v2 = 0;

        for (var i = 0, p = 10; (cpf.length - 2) > i; i++, p--) {
            v1 += cpf[i] * p;
        }
        
        v1 = (v1 * 10) % 11;

        if (v1 == 10) {
            v1 = 0;
        }

        if (v1 != cpf[9]) {
            return false;
        }

        for (var i = 0, p = 11; (cpf.length - 1) > i; i++, p--) {
            v2 += cpf[i] * p;
        }

        v2 = (v2 * 10) % 11;

        if (v2 == 10) {
            v2 = 0;
        }

        if (v2 != cpf[10]) {
            return false;
        } else {
            return true;
        }
    } else {
        return false;
    }
}