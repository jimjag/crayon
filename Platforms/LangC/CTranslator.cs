﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel.Nodes;

namespace LangC
{
    public abstract class CTranslator : Platform.CurlyBraceTranslator
    {
        public CTranslator(Platform.AbstractPlatform platform) : base(platform, "    ", "\n", false)
        { }

        public override void TranslateArrayGet(StringBuilder sb, Expression array, Expression index)
        {
            throw new NotImplementedException();
        }

        public override void TranslateArrayJoin(StringBuilder sb, Expression array, Expression sep)
        {
            throw new NotImplementedException();
        }

        public override void TranslateArrayLength(StringBuilder sb, Expression array)
        {
            throw new NotImplementedException();
        }

        public override void TranslateArrayNew(StringBuilder sb, PType arrayType, Expression lengthExpression)
        {
            throw new NotImplementedException();
        }

        public override void TranslateArraySet(StringBuilder sb, Expression array, Expression index, Expression value)
        {
            throw new NotImplementedException();
        }

        public override void TranslateCast(StringBuilder sb, PType type, Expression expression)
        {
            throw new NotImplementedException();
        }

        public override void TranslateCharConstant(StringBuilder sb, char value)
        {
            throw new NotImplementedException();
        }

        public override void TranslateCharToString(StringBuilder sb, Expression charValue)
        {
            throw new NotImplementedException();
        }

        public override void TranslateChr(StringBuilder sb, Expression charCode)
        {
            throw new NotImplementedException();
        }

        public override void TranslateCommandLineArgs(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        public override void TranslateConstructorInvocation(StringBuilder sb, ConstructorInvocation constructorInvocation)
        {
            throw new NotImplementedException();
        }

        public override void TranslateConvertRawDictionaryValueCollectionToAReusableValueList(StringBuilder sb, Expression dictionary)
        {
            throw new NotImplementedException();
        }

        public override void TranslateCurrentTimeSeconds(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        public override void TranslateDictionaryContainsKey(StringBuilder sb, Expression dictionary, Expression key)
        {
            throw new NotImplementedException();
        }

        public override void TranslateDictionaryGet(StringBuilder sb, Expression dictionary, Expression key)
        {
            throw new NotImplementedException();
        }

        public override void TranslateDictionaryKeys(StringBuilder sb, Expression dictionary)
        {
            throw new NotImplementedException();
        }

        public override void TranslateDictionaryKeysToValueList(StringBuilder sb, Expression dictionary)
        {
            throw new NotImplementedException();
        }

        public override void TranslateDictionaryNew(StringBuilder sb, PType keyType, PType valueType)
        {
            throw new NotImplementedException();
        }

        public override void TranslateDictionaryRemove(StringBuilder sb, Expression dictionary, Expression key)
        {
            throw new NotImplementedException();
        }

        public override void TranslateDictionarySet(StringBuilder sb, Expression dictionary, Expression key, Expression value)
        {
            throw new NotImplementedException();
        }

        public override void TranslateDictionarySize(StringBuilder sb, Expression dictionary)
        {
            throw new NotImplementedException();
        }

        public override void TranslateDictionaryValues(StringBuilder sb, Expression dictionary)
        {
            throw new NotImplementedException();
        }

        public override void TranslateDictionaryValuesToValueList(StringBuilder sb, Expression dictionary)
        {
            throw new NotImplementedException();
        }

        public override void TranslateFloatBuffer16(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        public override void TranslateFloatDivision(StringBuilder sb, Expression floatNumerator, Expression floatDenominator)
        {
            throw new NotImplementedException();
        }

        public override void TranslateFloatToInt(StringBuilder sb, Expression floatExpr)
        {
            throw new NotImplementedException();
        }

        public override void TranslateFloatToString(StringBuilder sb, Expression floatExpr)
        {
            throw new NotImplementedException();
        }

        public override void TranslateGetProgramData(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        public override void TranslateGetResourceManifest(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        public override void TranslateGlobalVariable(StringBuilder sb, Variable variable)
        {
            throw new NotImplementedException();
        }

        public override void TranslateIntBuffer16(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        public override void TranslateIntegerDivision(StringBuilder sb, Expression integerNumerator, Expression integerDenominator)
        {
            throw new NotImplementedException();
        }

        public override void TranslateIntToString(StringBuilder sb, Expression integer)
        {
            throw new NotImplementedException();
        }

        public override void TranslateInvokeDynamicLibraryFunction(StringBuilder sb, Expression functionId, Expression argsArray)
        {
            throw new NotImplementedException();
        }

        public override void TranslateIsValidInteger(StringBuilder sb, Expression stringValue)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListAdd(StringBuilder sb, Expression list, Expression item)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListClear(StringBuilder sb, Expression list)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListConcat(StringBuilder sb, Expression list, Expression items)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListGet(StringBuilder sb, Expression list, Expression index)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListInsert(StringBuilder sb, Expression list, Expression index, Expression item)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListJoinChars(StringBuilder sb, Expression list)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListJoinStrings(StringBuilder sb, Expression list, Expression sep)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListNew(StringBuilder sb, PType type)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListPop(StringBuilder sb, Expression list)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListRemoveAt(StringBuilder sb, Expression list, Expression index)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListReverse(StringBuilder sb, Expression list)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListSet(StringBuilder sb, Expression list, Expression index, Expression value)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListShuffle(StringBuilder sb, Expression list)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListSize(StringBuilder sb, Expression list)
        {
            throw new NotImplementedException();
        }

        public override void TranslateListToArray(StringBuilder sb, Expression list)
        {
            throw new NotImplementedException();
        }

        public override void TranslateMathArcCos(StringBuilder sb, Expression ratio)
        {
            throw new NotImplementedException();
        }

        public override void TranslateMathArcSin(StringBuilder sb, Expression ratio)
        {
            throw new NotImplementedException();
        }

        public override void TranslateMathArcTan(StringBuilder sb, Expression yComponent, Expression xComponent)
        {
            throw new NotImplementedException();
        }

        public override void TranslateMathCos(StringBuilder sb, Expression thetaRadians)
        {
            throw new NotImplementedException();
        }

        public override void TranslateMathLog(StringBuilder sb, Expression value)
        {
            throw new NotImplementedException();
        }

        public override void TranslateMathPow(StringBuilder sb, Expression expBase, Expression exponent)
        {
            throw new NotImplementedException();
        }

        public override void TranslateMathSin(StringBuilder sb, Expression thetaRadians)
        {
            throw new NotImplementedException();
        }

        public override void TranslateMathTan(StringBuilder sb, Expression thetaRadians)
        {
            throw new NotImplementedException();
        }

        public override void TranslateMultiplyList(StringBuilder sb, Expression list, Expression n)
        {
            throw new NotImplementedException();
        }

        public override void TranslateNullConstant(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        public override void TranslateParseFloatUnsafe(StringBuilder sb, Expression stringValue)
        {
            throw new NotImplementedException();
        }

        public override void TranslateParseInt(StringBuilder sb, Expression safeStringValue)
        {
            throw new NotImplementedException();
        }

        public override void TranslatePrintStdErr(StringBuilder sb, Expression value)
        {
            throw new NotImplementedException();
        }

        public override void TranslatePrintStdOut(StringBuilder sb, Expression value)
        {
            throw new NotImplementedException();
        }

        public override void TranslateRandomFloat(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        public override void TranslateReadByteCodeFile(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        public override void TranslateRegisterLibraryFunction(StringBuilder sb, Expression libRegObj, Expression functionName, Expression functionArgCount)
        {
            throw new NotImplementedException();
        }

        public override void TranslateResourceReadTextFile(StringBuilder sb, Expression path)
        {
            throw new NotImplementedException();
        }

        public override void TranslateSetProgramData(StringBuilder sb, Expression programData)
        {
            throw new NotImplementedException();
        }

        public override void TranslateSortedCopyOfIntArray(StringBuilder sb, Expression intArray)
        {
            throw new NotImplementedException();
        }

        public override void TranslateSortedCopyOfStringArray(StringBuilder sb, Expression stringArray)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringAppend(StringBuilder sb, Expression str1, Expression str2)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringBuffer16(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringCharAt(StringBuilder sb, Expression str, Expression index)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringCharCodeAt(StringBuilder sb, Expression str, Expression index)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringCompareIsReverse(StringBuilder sb, Expression str1, Expression str2)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringConcatAll(StringBuilder sb, Expression[] strings)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringConcatPair(StringBuilder sb, Expression strLeft, Expression strRight)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringContains(StringBuilder sb, Expression haystack, Expression needle)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringEndsWith(StringBuilder sb, Expression haystack, Expression needle)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringEquals(StringBuilder sb, Expression left, Expression right)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringFromCharCode(StringBuilder sb, Expression charCode)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringIndexOf(StringBuilder sb, Expression haystack, Expression needle)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringIndexOfWithStart(StringBuilder sb, Expression haystack, Expression needle, Expression startIndex)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringLength(StringBuilder sb, Expression str)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringReplace(StringBuilder sb, Expression haystack, Expression needle, Expression newNeedle)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringReverse(StringBuilder sb, Expression str)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringSplit(StringBuilder sb, Expression haystack, Expression needle)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringStartsWith(StringBuilder sb, Expression haystack, Expression needle)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringSubstring(StringBuilder sb, Expression str, Expression start, Expression length)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringSubstringIsEqualTo(StringBuilder sb, Expression haystack, Expression startIndex, Expression needle)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringToLower(StringBuilder sb, Expression str)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringToUpper(StringBuilder sb, Expression str)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringTrim(StringBuilder sb, Expression str)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringTrimEnd(StringBuilder sb, Expression str)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStringTrimStart(StringBuilder sb, Expression str)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStrongReferenceEquality(StringBuilder sb, Expression left, Expression right)
        {
            throw new NotImplementedException();
        }

        public override void TranslateStructFieldDereference(StringBuilder sb, Expression root, StructDefinition structDef, string fieldName, int fieldIndex)
        {
            throw new NotImplementedException();
        }

        public override void TranslateThreadSleep(StringBuilder sb, Expression seconds)
        {
            throw new NotImplementedException();
        }

        public override void TranslateTryParseFloat(StringBuilder sb, Expression stringValue, Expression floatOutList)
        {
            throw new NotImplementedException();
        }

        public override void TranslateVariableDeclaration(StringBuilder sb, VariableDeclaration varDecl)
        {
            throw new NotImplementedException();
        }

        public override void TranslateVmDetermineLibraryAvailability(StringBuilder sb, Expression libraryName, Expression libraryVersion)
        {
            throw new NotImplementedException();
        }

        public override void TranslateVmEnqueueResume(StringBuilder sb, Expression seconds, Expression executionContextId)
        {
            throw new NotImplementedException();
        }

        public override void TranslateVmRunLibraryManifest(StringBuilder sb, Expression libraryName, Expression libRegObj)
        {
            throw new NotImplementedException();
        }
    }
}
